using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using dotNETCoreRESTAPI.Extensions;
using dotNETCoreRESTAPI.Interfaces;
using dotNETCoreRESTAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace dotNETCoreRESTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AppSettings _appSettings;
        
        public AuthController(INotificator notificator,
                            SignInManager<IdentityUser> signInManager,
                            UserManager<IdentityUser> userManager,
                            IOptions<AppSettings> appSettings) : base(notificator)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appSettings = appSettings.Value;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register(NewUser newUser) 
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var user = new IdentityUser
            {
                UserName = newUser.Email,
                Email = newUser.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, newUser.Password);

            if(result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return CustomResponse(JWT(user.Email));
            }
            foreach(var error in result.Errors)
            {
                NotifyError(error.Description);
            }

            return CustomResponse(newUser);
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(Login login)
        {
            if(!ModelState.IsValid) return CustomResponse(ModelState);

            var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password,false,true);

            if(result.IsLockedOut)
            {
                NotifyError("User temporarily locked by invalid attemps");
            }

            if(!result.Succeeded)
            {
                NotifyError("User name or password is incorrect");
            }

            return CustomResponse(JWT(login.Email));
        }

        private async Task<LoginResponse> JWT(string email)
        {

            var user = await _userManager.FindByEmailAsync(email);
            var claims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, ToUnixEpochDate(DateTime.UtcNow).ToString()));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(DateTime.UtcNow).ToString(), ClaimValueTypes.Integer64));

            foreach(var role in roles)
            {
                claims.Add(new Claim("role", role));
            }

            var identityClaims = new ClaimsIdentity();
            identityClaims.AddClaims(claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            
            var key =  Encoding.ASCII.GetBytes(_appSettings.Secret);
            
            var token = tokenHandler.CreateToken(new SecurityTokenDescriptor
                {
                    Issuer = _appSettings.Issuer,
                    Audience = _appSettings.Audience,
                    Expires = DateTime.UtcNow.AddHours(_appSettings.Expires),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Subject = identityClaims
                }
            );

            var encodedToken = tokenHandler.WriteToken(token);

            var response = new LoginResponse
            {
                AccessToken = encodedToken,
                ExpiresIn = TimeSpan.FromHours(_appSettings.Expires).TotalSeconds,
                UserToken = new UserToken
                {
                    Id = user.Id,
                    Email = user.Email,
                    Claims = claims.Select(
                        options => new UserClaim
                        {
                            Type = options.Type,
                            Value = options.Value
                        }
                    )
                }
            };

            return response;
        }

        private static long ToUnixEpochDate(DateTime date)
            =>(long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970,1,1,0,0,0,TimeSpan.Zero)).TotalSeconds);
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotNETCoreRESTAPI.Interfaces;
using dotNETCoreRESTAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dotNETCoreRESTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : MainController
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        
        public AuthController(INotificator notificator,
                                SignInManager<IdentityUser> signInManager,
                                UserManager<IdentityUser> userManager) : base(notificator)
        {
            _signInManager = signInManager;
            _userManager = userManager;
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
                return CustomResponse(newUser);
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

            return CustomResponse(login);
        }
    }
}
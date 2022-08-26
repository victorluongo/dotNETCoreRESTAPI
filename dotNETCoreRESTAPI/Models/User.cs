using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace dotNETCoreRESTAPI.Models
{
    public class NewUser
    {
        [Required(ErrorMessage = "{0} is required.")]
        [EmailAddress(ErrorMessage = "{0} is invalid.")]
        public string Email {get; set;}

        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(100, ErrorMessage = "{0} can't be less than {2} or greater then {1}", MinimumLength = 6)]
        public string Password {get; set;}

        [Compare("Password", ErrorMessage ="Passwords doesn't match.")]
        public string ConfirmPassword {get; set;}        
    }

    public class Login
    {
        [Required(ErrorMessage = "{0} is required.")]
        [EmailAddress(ErrorMessage = "{0} is invalid.")]
        public string Email {get; set;}

        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(100, ErrorMessage = "{0} can't be less than {2} or greater then {1}", MinimumLength = 6)]
        public string Password {get; set;}

    }

    public class LoginResponse
    {
        public string AccessToken  {get; set;}
        public double ExpiresIn  {get; set;}
        public UserToken UserToken  {get; set;}
    }

    public class UserToken
    {
        public string Id {get; set;}
        public string Email  {get; set;}
        public IEnumerable<UserClaim> Claims  {get; set;}
    }

    public class UserClaim
    {
        public string Type  {get; set;}
        public string Value  {get; set;}
    }
}
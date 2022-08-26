using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using dotNETCoreRESTAPI.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace dotNETCoreRESTAPI.Extensions
{
    public class CustomAuthorization
    {
        public static bool UserClaims(HttpContext context, string claimName, string claimValue)
        {
            return context.User.Identity.IsAuthenticated && context.User.Claims.Any(
                options =>
                options.Type == claimName &&
                options.Value.Contains(claimValue)
            );
        }
    }

    public class ClaimsAuthorizeAttribute : TypeFilterAttribute
    {
        public ClaimsAuthorizeAttribute(string claimName, string claimValue) : base(typeof(ClaimFilter))
        {
            Arguments = new object[] {new Claim(
                claimName, 
                claimValue
                ) 
            };
        }
    }
}

public class ClaimFilter : IAuthorizationFilter
{
    private readonly Claim _claim;

    public ClaimFilter(Claim claim)
    {
        _claim = claim;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        if(!context.HttpContext.User.Identity.IsAuthenticated)
        {
            context.Result = new StatusCodeResult(401);
            return;
        }

        if(!CustomAuthorization.UserClaims(context.HttpContext, _claim.Type, _claim.Value))
        {
            context.Result = new StatusCodeResult(403);
        };
    }
}
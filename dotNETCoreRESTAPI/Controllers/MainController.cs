using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotNETCoreRESTAPI.Interfaces;
using dotNETCoreRESTAPI.Notifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace dotNETCoreRESTAPI.Controllers
{
    [ApiController]
    public class MainController : ControllerBase
    {

        private readonly INotificator _notificator;
        public readonly IUser _user;

        protected Guid userId {get; set;}
        protected string userName {get; set;}
        protected string userEmail {get; set;}
        protected bool userIsAuthenticated {get; set;}

        protected MainController(INotificator notificator, IUser user)
        {
            _notificator = notificator;
            _user = user;

            if(_user.IsAuthenticated())
            {
                userId = _user.GetUserId();
                userName = _user.Name;
                userEmail = _user.GetUserEmail();
                userIsAuthenticated = true;
            }
        }

        protected ActionResult CustomResponse(object result = null)
        {
            if (!_notificator.HasNotification())
            return Ok(new{
                success = true,
                data    = result
            });

            return BadRequest(new{
                success = false,
                errors  = _notificator.GetNotifications().Select(n => n.NotificationMessage)
            });
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if(!modelState.IsValid) NotifyInvalidModelState(modelState);
            return CustomResponse();
        }

        protected void NotifyInvalidModelState(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(e => e.Errors);
            foreach(var error in errors)
            {
                var errorMessage = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
                NotifyError(errorMessage);
            }
        }

        protected void NotifyError(string errorMessage)
        {
            _notificator.Handle(new Notification(errorMessage));
            
        }
    }
}
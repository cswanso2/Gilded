using Gilded.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Gilded.Filters
{
    public class ApiKeyFilter : AuthorizeAttribute
    {

        public ApiKeyFilter(IUserRepository repository)
        {
            _userRepository = repository;
        }

        private readonly IUserRepository _userRepository;

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (AuthorizeRequest(actionContext))
            {
                return;
            }
            HandleUnauthorizedRequest(actionContext);
        }

        private bool AuthorizeRequest(HttpActionContext actionContext)
        {
            try
            {
                var apiKey = actionContext.Request.Headers.Authorization.Parameter;
                var user = _userRepository.GetUser(apiKey);
                actionContext.Request.Properties["user"] = user;
                return true;
            }
            //user with api key doesn't exist
            catch(KeyNotFoundException)
            {
                return false;
            }
        }

    }
}
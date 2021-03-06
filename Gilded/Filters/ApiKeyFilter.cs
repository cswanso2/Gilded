﻿using Gilded.Repositories;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Gilded.Filters
{
    public class ApiKeyFilter : AuthorizeAttribute
    {
        private static readonly IUserRepository _userRepository = UserRepository.Get();
        private const string AdminRole = "Admin";
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
                if (actionContext.Request.Headers.Authorization == null)
                    return false;
                var apiKey = actionContext.Request.Headers.Authorization.Scheme;
                var user = _userRepository.GetUser(apiKey);
                actionContext.Request.Properties["user"] = user;
                if (Roles.Contains(AdminRole) && user.Role != AdminRole)
                    return false;
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
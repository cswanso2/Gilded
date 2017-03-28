using Gilded.Exceptions;
using Gilded.Filters;
using Gilded.Models;
using Gilded.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace Gilded.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private readonly IUserRepository _repository;
        public UsersController(IUserRepository repository)
        {
            _repository = repository;
        }

        public UsersController()
        {
            _repository = UserRepository.Get();
        }
        [HttpPost]
        [Route("register")]
        public HttpResponseMessage Register(string emailAddress)
        {
            try
            {
                var apiKey = _repository.CreateUser(emailAddress);
                var response = new HttpResponseMessage(HttpStatusCode.Created);
                response.Content = new StringContent(apiKey,
                        Encoding.UTF8, "application/json");
                return response;
            }
            catch(DuplicateUserException)
            {
                return new HttpResponseMessage(HttpStatusCode.Conflict);
            }
            catch(FormatException)
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }
        }

        [HttpPut]
        [Route("balances/{amount}")]
        [ApiKeyFilter]
        public HttpResponseMessage AddBalance(int amount)
        {
            var user = ActionContext.Request.Properties["user"] as User;
            _repository.AddBalance(user.ApiKey, amount);
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }
    }
}

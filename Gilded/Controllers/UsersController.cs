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
    public class UsersController : ApiController
    {
        private readonly IUserRepository _repository;
        [HttpPost]
        [Route("register")]
        public HttpResponseMessage Register([FromBody]string emailAddress)
        {
            var apiKey = _repository.CreateUser(emailAddress);
            var response =  new HttpResponseMessage(HttpStatusCode.Created);
            response.Content = new StringContent(apiKey,
                    Encoding.UTF8, "application/json");
            return response;
        }

        [HttpPut]
        [Route("/balances/{amount}")]
        [ApiKeyFilter]
        public HttpResponseMessage AddBalance(int amount)
        {
            var user = ActionContext.Request.Properties["user"] as User;
            _repository.AddBalance(user.EmailAddress, amount);
            throw new NotImplementedException();
        }
    }
}

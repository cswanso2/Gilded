using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Gilded.Controllers
{
    public class UsersController : ApiController
    {
        [HttpPost]
        [Route("register")]
        public IHttpActionResult Register(string userName, string password)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login(string userName, string password)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("users/{itemName}/balnces/{amount}")]
        public IHttpActionResult PurchaseItem(string userName, int balance)
        {
            throw new NotImplementedException();
        }
    }
}

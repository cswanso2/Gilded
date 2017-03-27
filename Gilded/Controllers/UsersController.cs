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
        public IHttpActionResult Register([FromBody]string emailAddress)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("/balnces/{amount}")]
        public IHttpActionResult AddBalance(int amount)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Gilded.Controllers
{
    public class ItemsController : ApiController
    {
        public IHttpActionResult Get()
        {
            throw new NotImplementedException();
        }

        public IHttpActionResult Post(string name, string description, int price)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("items/{itemName}/users/{userName}")]
        public IHttpActionResult PurchaseItem(string itemName, string userName)
        {
            throw new NotImplementedException();
        }
    }
}

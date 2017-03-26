using Gilded.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Gilded.Controllers
{
    [RoutePrefix("api/items")]
    public class ItemsController : ApiController
    {

        public IHttpActionResult Get()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public IHttpActionResult Post(Item item)
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

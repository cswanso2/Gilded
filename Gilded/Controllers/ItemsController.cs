using Gilded.Filters;
using Gilded.Models;
using Gilded.Repositories;
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
        private readonly IItemRepository _itemsRepostory;
        public List<Item> Get()
        {
            return _itemsRepostory.GetItems();
        }

        [HttpPost]
        [ApiKeyFilter(Roles="Admin")]
        public HttpResponseMessage Post(Item item)
        {
            _itemsRepostory.CreateItem(item);
            return new HttpResponseMessage();
        }

        [HttpPost]
        [Route("items/{itemName}/users/")]
        public HttpResponseMessage PurchaseItem(string itemName)
        {
            throw new NotImplementedException();
        }
    }
}

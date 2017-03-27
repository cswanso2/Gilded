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

        public ItemsController()
        {
            _itemsRepostory = null;
        }

        public ItemsController(IItemRepository itemRepository)
        {
            _itemsRepostory = itemRepository;
        }

        public List<Item> Get()
        {
            return _itemsRepostory.GetItems();
        }

        /*
         * To update item simply post the same item
         */
        [HttpPut]
        [ApiKeyFilter(Roles="Admin")]
        public HttpResponseMessage Put(Item item)
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

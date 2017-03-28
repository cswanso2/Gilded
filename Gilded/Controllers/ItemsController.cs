using Gilded.Exceptions;
using Gilded.Filters;
using Gilded.Managers;
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
        private readonly IPurchaseManager _purchaseManager;
        public ItemsController()
        {
            _itemsRepostory = ItemRepository.Get();
            _purchaseManager = new PurchaseManager(ItemRepository.Get(), UserRepository.Get());
        }

        public ItemsController(IItemRepository itemRepository, IPurchaseManager purchaseManager)
        {
            _itemsRepostory = itemRepository;
            _purchaseManager = purchaseManager;
        }


        public List<Item> Get()
        {
            return _itemsRepostory.GetItems();
        }

        /*
         * To update item simply put the same item
         */
        [HttpPut]
        [ApiKeyFilter(Roles="Admin")]
        public HttpResponseMessage Put(Item item)
        {
            _itemsRepostory.CreateItem(item);
            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        [HttpPut]
        [ApiKeyFilter(Roles = "Admin")]
        [Route("{itemName}/inventory/{numberOfNewItems}")]
        public HttpResponseMessage UpdateInventory(string itemName, int numberOfNewItems)
        {
            _itemsRepostory.ChangeInventory(itemName, numberOfNewItems);
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }

        [HttpPost]
        [Route("{itemName}/users/")]
        public HttpResponseMessage PurchaseItem(string itemName)
        {
            try
            {
                var user = ActionContext.Request.Properties["user"] as User;
                _purchaseManager.PurchaseItem(itemName, user);
                return new HttpResponseMessage(HttpStatusCode.Accepted);
            }
            catch(InsufficientFundsException)
            {
                return new HttpResponseMessage(HttpStatusCode.PaymentRequired);
            }
            catch (NoInventoryException)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
        }
    }
}

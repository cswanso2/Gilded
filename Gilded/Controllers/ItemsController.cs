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

        [HttpPost]
        [ApiKeyFilter(Roles="Admin")]
        public HttpResponseMessage Post(Item item)
        {
            try
            {
                _itemsRepostory.CreateItem(item);
            }
            catch(ArgumentException)
            {
                return new HttpResponseMessage(HttpStatusCode.Conflict);
            }
            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        [HttpPut]
        [ApiKeyFilter(Roles = "Admin")]
        [Route("{itemName}/inventories/{numberOfNewItems}")]
        public HttpResponseMessage UpdateInventory(string itemName, int numberOfNewItems)
        {
            try
            {
                _itemsRepostory.ChangeInventory(itemName, numberOfNewItems);
            }
            catch(NoInventoryException)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }
            return new HttpResponseMessage(HttpStatusCode.Accepted);
        }

        [HttpPost]
        [Route("{itemName}/purchases/")]
        [ApiKeyFilter]
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

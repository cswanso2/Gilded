using Gilded.Controllers;
using Gilded.Exceptions;
using Gilded.Managers;
using Gilded.Models;
using Gilded.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Controller
{
    public class ItemsControllerTests
    {
        private ItemsController _itemController;

        private Mock<IItemRepository> _mockRepository;
        private Mock<IPurchaseManager> _mockManager;

        private readonly User _user = new User()
        {
            ApiKey = "bla",
            Balance = 5,
            EmailAddress = "bla@bla.com",
            Role = "bla"
        };
        
        private readonly Item _item = new Item()
        {
            Price = 5,
            Name = "test item",
            Description = "testing 123"
        };

        private const string ApiKey = "abc";

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new Mock<IItemRepository>();
            _mockManager = new Mock<IPurchaseManager>();
            _itemController = new ItemsController(_mockRepository.Object, _mockManager.Object);
            _itemController.Request = new HttpRequestMessage();
            _itemController.ActionContext.Request.Properties["user"] = _user;
        }

        [Test]
        public void CreateItem()
        {
            var response = _itemController.Put(_item);
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
            _mockRepository.Verify(x => x.CreateItem(It.Is<Item>(y => y.Name == _item.Name)));
        }

        [Test]
        public void GetItems()
        {
            _mockRepository.Setup(x => x.GetItems()).Returns(new List<Item>() { _item });
            var response = _itemController.Get();
            Assert.AreEqual(1, response.Count);
            Assert.AreEqual(_item.Name, response[0].Name);
        }

        [Test]
        [TestCase("test item")]
        public void PurhchaseItem(string itemName)
        {
            
            var response = _itemController.PurchaseItem(itemName);
            _mockManager.Verify(x => x.PurchaseItem(itemName, It.Is<User>(y => y.ApiKey == _user.ApiKey)));
            Assert.AreEqual(HttpStatusCode.Accepted, response.StatusCode);
        }

        [Test]
        [TestCase("test item")]
        public void PurhchaseItemInsufficientFunds(string itemName)
        {
            _mockManager.Setup(x => x.PurchaseItem(itemName, It.Is<User>(y => y.ApiKey == _user.ApiKey))).Throws<InsufficientFundsException>();
            _itemController.Request = new HttpRequestMessage();
            _itemController.ActionContext.Request.Properties["user"] = _user;
            var response = _itemController.PurchaseItem(itemName);
            Assert.AreEqual(HttpStatusCode.PaymentRequired, response.StatusCode);
        }

        [Test]
        [TestCase("test item")]
        public void PurchaseItemNoInventory(string itemName)
        {
            _mockManager.Setup(x => x.PurchaseItem(itemName, It.Is<User>(y => y.ApiKey == _user.ApiKey))).Throws<NoInventoryException>();
            _itemController.Request = new HttpRequestMessage();
            _itemController.ActionContext.Request.Properties["user"] = _user;
            var response = _itemController.PurchaseItem(itemName);
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}

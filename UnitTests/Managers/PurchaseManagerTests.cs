using Gilded.Exceptions;
using Gilded.Managers;
using Gilded.Models;
using Gilded.Repositories;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.Managers
{
    public class PurchaseManagerTests
    {
        private Mock<IUserRepository> _userRepository;
        private Mock<IItemRepository> _itemRepository;
        private PurchaseManager _purchaseManager;

        private readonly User _user = new User() { ApiKey = "bla",
            EmailAddress = "bla@bla.com",
            Balance = 5
        };

        private readonly Item _item = new Item()
        {
            Name = "testItem",
            Description = "test description",
            Price = 5
        };

        private readonly Item _itemInvalidPrice = new Item()
        {
            Name = "testItemInvalidPrice",
            Description = "test description",
            Price = 6
        };
        [SetUp]
        public void SetUp()
        {
            _userRepository = new Mock<IUserRepository>();
            _itemRepository = new Mock<IItemRepository>();
            _purchaseManager = new PurchaseManager(_itemRepository.Object, _userRepository.Object);
        }

        [Test]
        [TestCase("testItem")]
        public void TestPurchase(string itemName)
        {
            _itemRepository.Setup(x => x.GetItem(itemName)).Returns(_item);
            _purchaseManager.PurchaseItem(itemName, _user);
            _userRepository.Verify(x => x.AddBalance(_user.ApiKey, _item.Price * -1));
            _itemRepository.Verify(x => x.ChangeInventory(itemName, -1));
        }

        [Test]
        [TestCase("testItemInvalidPrice")]
        public void TestPurchaseInsufficientFunds(string itemName)
        {
            _itemRepository.Setup(x => x.GetItem(itemName)).Returns(_itemInvalidPrice);
            Assert.Throws<InsufficientFundsException>(() => _purchaseManager.PurchaseItem(itemName, _user));
        }
    }
}

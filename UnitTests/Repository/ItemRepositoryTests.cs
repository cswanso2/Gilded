using Gilded.Repositories;
using Gilded.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Internal;
using Gilded.Exceptions;

namespace UnitTests.Repository
{
    public class ItemRepositoryTests
    {

        private ItemRepository _itemRepository;

        /*
         * I would much prefer not to call multiple functions in one unit test, 
         * I think of these more like service tests for the database
         */

        [SetUp]
        public void SetUp()
        {
            ItemRepository.Clear();
            _itemRepository = ItemRepository.Get();
        }

        private readonly Item _testItem = new Item()
        {
            Description = "Description",
            Name = "TestItem",
            Price = 25
        };

        private readonly Item _testItemTwo = new Item()
        {
            Description = "DescriptionTwo",
            Name = "TestItemTwo",
            Price = 10
        };

        [Test]
        public void CreateGetNoInventory()
        {
            _itemRepository.CreateItem(_testItem);
            Assert.Throws<NoInventoryException>(() => _itemRepository.GetItem(_testItem.Name));
        }

        [Test]
        public void CreateTwice()
        {
            _itemRepository.CreateItem(_testItem);
            Assert.Throws<ArgumentException>(() => _itemRepository.CreateItem(_testItem));
        }

        [Test]
        public void NoInventory()
        {
            Assert.Throws<NoInventoryException>(() => _itemRepository.GetItem(_testItem.Name));
        }

        [Test]
        public void CreateGetItemIncreaseInventory()
        {
            _itemRepository.CreateItem(_testItem);
            _itemRepository.ChangeInventory(_testItem.Name, 1);
            var item = _itemRepository.GetItem(_testItem.Name);
            Assert.AreEqual(_testItem.Name, item.Name);
        }

        [Test]
        public void CreateGetItemsIncreaseInventory()
        {
            _itemRepository.CreateItem(_testItem);
            _itemRepository.ChangeInventory(_testItem.Name, 1);
            _itemRepository.CreateItem(_testItemTwo);
            _itemRepository.ChangeInventory(_testItemTwo.Name, 1);
            var items = _itemRepository.GetItems();
            Assert.AreEqual(items.Count, 2);
            Assert.IsTrue(items.Any(x => x.Name == _testItem.Name));
            Assert.IsTrue(items.Any(x => x.Name == _testItemTwo.Name));

        }
    }
}

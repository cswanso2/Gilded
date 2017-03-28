using System;
using System.Collections.Generic;
using System.Linq;
using Gilded.Models;
using Gilded.Exceptions;

namespace Gilded.Repositories
{
    public class ItemRepository : SingletonRepository<ItemRepository>, IItemRepository
    {
        private class ItemInventory
        {
            public int InventoryAmount { get; set; }
            public Item Item { get; set; }

        }
        private Dictionary<string, ItemInventory> _inventory = new Dictionary<string, ItemInventory>();

        public void ChangeInventory(string itemName, int amountChanged)
        {
            var itemInventory = _inventory[itemName];
            itemInventory.InventoryAmount += amountChanged;
        }

        public void CreateItem(Item item)
        {
            var itemInventory = new ItemInventory
            {
                InventoryAmount = 0,
                Item = item
            };
            _inventory.Add(item.Name, itemInventory);
        }

        public Item GetItem(string itemName)
        {
            if (!_inventory.ContainsKey(itemName))
                throw new NoInventoryException();
            var itemInventory = _inventory[itemName];
            if (itemInventory.InventoryAmount > 0)
                return itemInventory.Item;
            throw new NoInventoryException();
        }

        public List<Item> GetItems()
        {
            return _inventory.Values.Where(x => x.InventoryAmount > 0).Select(x => x.Item).ToList();
        }
    }
}
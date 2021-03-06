﻿using Gilded.Models;
using System.Collections.Generic;
namespace Gilded.Repositories
{
    public interface IItemRepository
    {
        /*
         * Creates an item, throws an exception on non unique names
         */
        void CreateItem(Item item);

        /*
         * Creates an item, throws an exception on non unique names
         */
        void ChangeInventory(string itemName, int amountChanged);

        /*
         * Gets all items
         */
        List<Item> GetItems();

        Item GetItem(string itemName);
    }
}

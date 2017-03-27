using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gilded.Models;
using Gilded.Repositories;
using Gilded.Exceptions;

namespace Gilded.Managers
{
    public class PurchaseManager : IPurchaseManager
    {
        private readonly IItemRepository _itemRepository;
        private readonly IUserRepository _userRepository;
        /*
         *Concurrency safety for this is imo the biggest challenge of the project,
         * IE user buys two items at once. Or item with one left gets purchased twice.
         * I took a bit of the easy way out and am only allowing one user to purchase a item at time
         */
        private readonly object _lock = new object();
        public PurchaseManager(IItemRepository itemRepository, IUserRepository userRepository)
        {
            _itemRepository = itemRepository;
            _userRepository = userRepository;
        }

        public void PurchaseItem(string itemName, User user)
        {
            lock(_lock)
            {
                var item = _itemRepository.GetItem(itemName);
                if (item.Price > user.Balance)
                    throw new InsufficientFundsException();
                _itemRepository.ChangeInventory(item.Name, -1);
                _userRepository.AddBalance(user.EmailAddress, item.Price * -1);
            }
        }

    }
}
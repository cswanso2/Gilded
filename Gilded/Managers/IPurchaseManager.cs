using Gilded.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gilded.Managers
{
    public interface IPurchaseManager
    {
        /*
         * Used to purchase an item
         */
        void PurhcaseItem(string itemName, User user);

    }
}

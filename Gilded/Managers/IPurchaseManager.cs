using Gilded.Models;
namespace Gilded.Managers
{
    public interface IPurchaseManager
    {
        /*
         * Used to purchase an item
         */
        void PurchaseItem(string itemName, User user);

    }
}

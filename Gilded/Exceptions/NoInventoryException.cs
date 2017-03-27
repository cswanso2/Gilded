using System;
namespace Gilded.Exceptions
{
    public class NoInventoryException : Exception
    {
        public NoInventoryException() : base("No more of the inventory is in stock")
        {

        }
    }
}
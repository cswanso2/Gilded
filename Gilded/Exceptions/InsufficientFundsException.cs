using System;
namespace Gilded.Exceptions
{
    public class InsufficientFundsException : Exception
    {
        public InsufficientFundsException() : base("User does not have sufficient funds to purchase this item")
        {

        }
    }
}
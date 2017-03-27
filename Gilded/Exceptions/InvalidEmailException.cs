using System;
namespace Gilded.Exceptions
{
    public class InvalidEmailException : Exception
    {
        public InvalidEmailException(): base("Email is in an invalid format")
        {
        }
    }
}
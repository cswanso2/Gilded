using System;
namespace Gilded.Exceptions
{
    public class DuplicateUserException : Exception
    {
        public DuplicateUserException() : base("User already exists")
        {

        }
    }
}
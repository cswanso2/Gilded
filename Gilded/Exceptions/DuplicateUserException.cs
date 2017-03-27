using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gilded.Exceptions
{
    public class DuplicateUserException : Exception
    {
        public DuplicateUserException() : base("User already exists")
        {

        }
    }
}
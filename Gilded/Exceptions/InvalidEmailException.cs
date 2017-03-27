using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gilded.Exceptions
{
    public class InvalidEmailException : Exception
    {
        public InvalidEmailException(): base("Email is in an invalid format")
        {
        }
    }
}
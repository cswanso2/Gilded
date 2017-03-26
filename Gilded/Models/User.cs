using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gilded.Models
{
    public class User
    {
        //will be unique identifier
        public string UserName { get; set; }
        public int Balance { get; set; }
    }
}
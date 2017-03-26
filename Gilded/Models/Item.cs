using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gilded.Models
{
    public class Item
    {
        //Will be assumed to be unique and act as primary key.
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
    }
}
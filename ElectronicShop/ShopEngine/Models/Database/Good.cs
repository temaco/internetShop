using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopEngine.Models.Database
{
    public class Good
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public DateTime DateTime { get; set; }
        public User Creator { get; set; }
        public string ImageName { get; set; }
    }
}
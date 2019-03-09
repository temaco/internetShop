using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopEngine.Models.Database
{
    public class Basket
    {
        public int ID { get; set; }
        public User User { get; set; }
        public Good Good { get; set; }
        public int Count { get; set; }
    }
}
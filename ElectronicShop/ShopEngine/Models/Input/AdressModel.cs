using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopEngine.Models.Input
{
    public class AdressModel
    {
        public int Transaction { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public int Apartement { get; set; }
    }
}
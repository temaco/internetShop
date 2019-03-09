using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopEngine.Models.Database
{
    public class Adress
    {
        [Key]
        public int ID { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string House { get; set; }
        public int Apartement { get; set; }
        public int Days { get; set; }
    }
}
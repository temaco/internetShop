using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopEngine.Models.Input
{
    public class TransactionInformation
    {
        public string Card { get; set; }
        public string CardSecurity { get; set; }
        public DateTime CardDateTime { get; set; }
        public string Name { get; set; }
        public int UserID { get; set; }
        public int TransactionID { get; set; }
    }
}
using ShopEngine.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicShop.Models
{
    public class GoodWorker
    {
        public string Action { get; set; }
        public GoodModel GoodModel { get; set; }
    }
}

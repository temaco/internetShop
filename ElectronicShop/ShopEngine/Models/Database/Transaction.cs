using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopEngine.Models.Database
{

    public class TransactionGoods
    {
        [Key]
        public int ID { get; set; }
        public string Good { get; set; }
        public User Creator { get; set; }
        public Transaction Transaction { get; set; }
        public int Count { get; set; }
        public double Price { get; set; }
    }

    public class Transaction
    {
        [Key]
        public int ID { get; set; }
        public DateTime DateTime { get; set; }
        public DateTime TimeDelivery { get; set; }
        public double Currency { get; set; }
        public User User { get; set; }
        public User DeliveryMan { get; set; }
        public bool IsClose { get; set; }
        public bool IsCancel { get; set; }
        public bool IsDeliver { get; set; }
        public bool? IsUserDeliver { get; set; }
        public Adress Adress { get; set; }
        public List<TransactionGoods> TransactionGoods { get; set; }
    }
}
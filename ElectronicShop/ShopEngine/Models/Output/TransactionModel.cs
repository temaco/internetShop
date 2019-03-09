using ShopEngine.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ShopEngine.Models.Output
{
    [DataContract]
    public class TransactionModel : IEngineServiceModel
    {
        public TransactionModel(Transaction transaction)
        {
            ActualID = transaction.ID;
            IsUserDelivery = transaction.IsUserDeliver;
            Currency = transaction.Currency;
            DateTime = transaction.DateTime;
            IsClose = transaction.IsClose;
            IsCancel = transaction.IsCancel;
            IsDeliver = transaction.IsDeliver;
            OperationCode = OperationCode.Success;
            UserID = transaction.User.ID;
            TimeDelivery = transaction.TimeDelivery;
            GoodModels = transaction.TransactionGoods.Select(x => new GoodModel
            {
                ActualID = x.ID,
                Count = x.Count,
                Name = x.Good,
                OperationCode = OperationCode.Success,
                Price = x.Price
            }).ToList();
            if (transaction.Adress != null)
                this.Adress = $"{transaction.Adress.City}, {transaction.Adress.Street}, {transaction.Adress.House}, {transaction.Adress.Apartement}";
        }

        [DataMember]
        public string Adress;
        [DataMember]
        public int ActualID { get; set; }
        [DataMember]
        public OperationCode OperationCode { get; set; }
        [DataMember]
        public DateTime DateTime { get; set; }
        [DataMember]
        public double Currency { get; set; }
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public bool IsClose { get; set; }
        [DataMember]
        public bool IsCancel { get; set; }
        [DataMember]
        public bool IsDeliver { get; set; }
        [DataMember]
        public bool? IsUserDelivery { get; set; }
        [DataMember]
        public DateTime TimeDelivery { get; set; }
        [DataMember]
        public List<GoodModel> GoodModels { get; set; }
    }
}
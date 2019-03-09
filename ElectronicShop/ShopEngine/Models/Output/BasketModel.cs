using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ShopEngine.Models.Output
{
    [DataContract]
    public class BasketModel : IEngineServiceModel
    {
        [DataMember]
        public int ActualID { get; set; }
        [DataMember]
        public OperationCode OperationCode { get; set; }
        [DataMember]
        public GoodModel GoodModel { get; set; }
        [DataMember]
        public int UserID { get; set; }
        [DataMember]
        public int Count { get; set; }
    }
}
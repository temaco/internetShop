using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace ShopEngine.Models.Output
{
    [DataContract]
    public class GoodModel : IEngineServiceModel
    {
        [DataMember]
        public int ActualID { get; set; }
        [DataMember]
        public OperationCode OperationCode { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Count { get; set; }
        [DataMember]
        public DateTime DateTime { get; set; }
        [DataMember]
        public double Price { get; set; }
        [DataMember]
        public string Creator { get; set; }
        [DataMember]
        public int CreatorID { get; set; }
        [DataMember]
        public string ImagePath { get; set; }
    }
}
using AccountService.Models.Output;
using ShopEngine.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ElectronicShop.Models
{
    [DataContract]
    public class UserModel : IAccountServiceResult
    {
        [DataMember]
        public int ActualID { get; set; }
        [DataMember]
        public OperationCode OperationCode { get; set; }
        [DataMember]
        public UserType UserType { get; set; }
        [DataMember]
        public string Login { get; set; }
    }
}

using ShopEngine.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AccountService.Models.Output
{
    [DataContract]
    public class LoginServiceResult : IAccountServiceResult
    {
        [DataMember]
        public int ActualID { get; set; }
        [DataMember]
        public OperationCode OperationCode { get; set; }
        [DataMember]
        public string Login { get; set; }
        [DataMember]
        public UserType UserType { get; set; }
    }
}
using AccountService.Models.Input;
using AccountService.Models.Output;
using ElectronicShop.Models;
using ShopEngine.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace AccountService
{
    [ServiceContract]
    public interface IAccountService
    {
        [OperationContract]
        IAccountServiceResult Login(LoginInputModels loginInputModels);
        [OperationContract]
        IAccountServiceResult Register(RegisterInputModel registerInputModel);
        [OperationContract]
        bool ChangeRole(int id, UserType userType);
        [OperationContract]
        List<UserModel> GetUserModels();
    }
}

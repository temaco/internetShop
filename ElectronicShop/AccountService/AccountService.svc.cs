using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using AccountService.Models;
using AccountService.Models.Input;
using AccountService.Models.Output;
using AccountService.Service;
using ElectronicShop.Models;
using ShopEngine.Models.Database;

namespace AccountService
{
    public class AccountService : IAccountService
    {
        public bool ChangeRole(int id, UserType userType)
        {
            DatabaseContext databaseContext = new DatabaseContext();
            var user = databaseContext.Users.Find(id);
            user.UserType = userType;
            databaseContext.Update(user);
            databaseContext.SaveChanges();
            return true;
        }

        public List<UserModel> GetUserModels()
        {
            return new DatabaseContext().Users.Select(x => new UserModel
            {
                ActualID = x.ID,
                OperationCode = OperationCode.Success,
                UserType = x.UserType,
                Login = x.Login
            }).ToList();
        }

        public IAccountServiceResult Login(LoginInputModels loginInputModels)
        {
            DatabaseContext databaseContext = new DatabaseContext();
            var find = databaseContext.Users.FirstOrDefault(x => x.Login == loginInputModels.Login
                                                                && x.PasswordHash == CryptService.MD5Hash(loginInputModels.Password));
            if (find == null)
                return new LoginServiceResult { OperationCode = OperationCode.NotFound };
            return new LoginServiceResult
            {
                OperationCode = OperationCode.Success,
                ActualID = find.ID,
                Login = find.Login,
                UserType = find.UserType
            };
        }

        public IAccountServiceResult Register(RegisterInputModel registerInputModel)
        {
            if (registerInputModel.Password != registerInputModel.PasswordHash)
                return new LoginServiceResult { OperationCode = OperationCode.ConfirmedError };
            DatabaseContext databaseContext = new DatabaseContext();
            var find = databaseContext.Users.FirstOrDefault(x => x.Login == registerInputModel.Login);
            if (find != null)
                return new LoginServiceResult { OperationCode = OperationCode.FoundCopy };
            var userNew = new User
            {
                Login = registerInputModel.Login,
                PasswordHash = CryptService.MD5Hash(registerInputModel.Password),
                UserType = registerInputModel.UserType,
                Name = registerInputModel.Name
            };
            databaseContext.Add(userNew);
            databaseContext.SaveChanges();
            return new LoginServiceResult
            {
                ActualID = userNew.ID,
                Login = userNew.Login,
                OperationCode = OperationCode.Success,
                UserType = userNew.UserType
            };
        }
    }
}

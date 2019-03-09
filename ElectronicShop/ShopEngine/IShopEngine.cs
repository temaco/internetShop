using ShopEngine.Models.Database;
using ShopEngine.Models.Input;
using ShopEngine.Models.Output;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ShopEngine
{
    [ServiceContract]
    public interface IShopEngine
    {
        [OperationContract]
        List<TransactionModel> DeliveryHistory(int id);

        [OperationContract]
        List<TransactionModel> DeliveryTransactions(int id);

        [OperationContract]
        void CancelTransactions(int id);

        [OperationContract]
        void DeliverySuccess(int id);

        [OperationContract]
        void DeliveryUser(int id, bool success);

        [OperationContract]
        List<TransactionModel> Transactions(int id);

        [OperationContract]
        bool SetAdress(AdressModel adressModel);

        [OperationContract]
        bool AddGood(GoodModel good);

        [OperationContract]
        bool DeleteGood(int id);

        [OperationContract]
        List<GoodModel> GetGoods();

        [OperationContract]
        List<GoodModel> GetGoodsForCreator(int creator);

        [OperationContract]
        GoodModel GetGoodByID(int id);

        [OperationContract]
        List<BasketModel> GetBaskets(int user);

        [OperationContract]
        bool DeleteFromBasket(int user, int basket);

        [OperationContract]
        bool AddGoodToBasket(int good, int user, int count);

        [OperationContract]
        TransactionModel StartTransactionByBasket(int user);

        [OperationContract]
        bool TransactionInformation(TransactionInformation transactionInformation);

        [OperationContract]
        bool UpdateGood(GoodModel good);
    }

}

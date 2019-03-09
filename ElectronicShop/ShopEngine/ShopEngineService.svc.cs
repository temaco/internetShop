using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ShopEngine.Models;
using ShopEngine.Models.Database;
using ShopEngine.Models.Input;
using ShopEngine.Models.Output;

namespace ShopEngine
{
    public class ShopEngineService : IShopEngine
    {
        public bool AddGood(GoodModel good)
        {
            DatabaseContextEngine databaseContextEngine = new DatabaseContextEngine();
            var user = databaseContextEngine.Users.Find(good.CreatorID);
            databaseContextEngine.Goods.Add(new Good
            {
                Creator = user,
                DateTime = DateTime.Now,
                ImageName = good.ImagePath,
                Name = good.Name,
                Price = good.Price
            });
            databaseContextEngine.SaveChanges();
            return true;
        }

        public bool AddGoodToBasket(int goodID, int userID, int count)
        {
            DatabaseContextEngine databaseContext = new DatabaseContextEngine();
            var good = databaseContext.Goods.Find(goodID);
            var user = databaseContext.Users.Find(userID);
            databaseContext.Baskets.Add(new Basket { Good = good, User = user, Count = count });
            databaseContext.SaveChanges();
            return true;
        }

        public void CancelTransactions(int id)
        {
            DatabaseContextEngine databaseContextEngine = new DatabaseContextEngine();
            var transaction = databaseContextEngine.Transactions.Find(id);
            transaction.IsCancel = true;
            databaseContextEngine.Update(transaction);
            databaseContextEngine.SaveChanges();
        }

        public bool TransactionInformation(TransactionInformation transactionInformation)
        {
            DatabaseContextEngine databaseContext = new DatabaseContextEngine();
            databaseContext.RemoveRange(databaseContext.Baskets.Where(x => x.User.ID == transactionInformation.UserID));
            databaseContext.SaveChanges();
            return true;
        }

        public bool DeleteFromBasket(int user, int basket)
        {
            DatabaseContextEngine databaseContext = new DatabaseContextEngine();
            var basketDB = databaseContext.Baskets.Find(basket);
            if (basketDB == null)
                return false;
            databaseContext.Remove(basketDB);
            databaseContext.SaveChanges();
            return true;
        }

        public bool DeleteGood(int id)
        {
            DatabaseContextEngine databaseContext = new DatabaseContextEngine();
            var good = databaseContext.Goods.Find(id);
            if (good == null)
                return false;
            databaseContext.Remove(good);
            databaseContext.SaveChanges();
            return true;
        }

        public void DeliverySuccess(int id)
        {
            DatabaseContextEngine databaseContextEngine = new DatabaseContextEngine();
            var transaction = databaseContextEngine.Transactions.Find(id);
            transaction.IsDeliver = true;
            databaseContextEngine.Update(transaction);
            databaseContextEngine.SaveChanges();
        }

        public void DeliveryUser(int id, bool sucess)
        {
            DatabaseContextEngine databaseContextEngine = new DatabaseContextEngine();
            var transaction = databaseContextEngine.Transactions.Find(id);
            transaction.IsUserDeliver = sucess;
            databaseContextEngine.Update(transaction);
            databaseContextEngine.SaveChanges();
        }

        public List<BasketModel> GetBaskets(int user)
        {
            DatabaseContextEngine databaseContext = new DatabaseContextEngine();
            return databaseContext.Baskets.Where(x => x.User.ID == user).Select(x => new BasketModel
            {
                ActualID = x.ID,
                UserID = x.User.ID,
                GoodModel = new GoodModel
                {
                    ActualID = x.Good.ID,
                    Price = x.Good.Price,
                    OperationCode = OperationCode.Success,
                    Creator = x.Good.Creator.Name,
                    DateTime = x.Good.DateTime,
                    Name = x.Good.Name
                },
                OperationCode = OperationCode.Success,
                Count = x.Count
            }).ToList();
        }

        public GoodModel GetGoodByID(int id)
        {
            DatabaseContextEngine databaseContextEngine = new DatabaseContextEngine();
            var good = databaseContextEngine.Goods.Include(x => x.Creator).FirstOrDefault(x => x.ID == id);
            if (good == null)
                return null;
            var creator = databaseContextEngine.Users.Find(good.Creator.ID);
            return new GoodModel
            {
                ActualID = good.ID,
                Creator = creator.Name,
                DateTime = good.DateTime,
                Name = good.Name,
                OperationCode = OperationCode.Success,
                Price = good.Price,
                ImagePath = good.ImageName
            };
        }

        public List<GoodModel> GetGoods()
        {
            DatabaseContextEngine databaseContext = new DatabaseContextEngine();
            return databaseContext.Goods
                .Select(x => new GoodModel
                {
                    ActualID = x.ID,
                    Creator = x.Creator.Name,
                    CreatorID = x.Creator.ID,
                    DateTime = x.DateTime,
                    Name = x.Name,
                    OperationCode = OperationCode.Success,
                    Price = x.Price,
                    ImagePath = x.ImageName
                }).ToList();
        }

        public List<GoodModel> GetGoodsForCreator(int creator)
        {
            DatabaseContextEngine databaseContext = new DatabaseContextEngine();
            return databaseContext.Goods.Where(x => x.Creator.ID == creator)
                .Select(x => new GoodModel
                {
                    ActualID = x.ID,
                    Creator = x.Creator.Name,
                    DateTime = x.DateTime,
                    Name = x.Name,
                    OperationCode = OperationCode.Success,
                    Price = x.Price,
                    ImagePath = x.ImageName
                }).ToList();
        }

        public bool SetAdress(AdressModel adressModel)
        {
            var database = new DatabaseContextEngine();
            var adress = database.Adresses.FirstOrDefault(x => x.City == adressModel.City
                                                            && x.Apartement == adressModel.Apartement
                                                            && x.House == adressModel.House
                                                            && x.Street == adressModel.Street);
            if (adress == null)
                return false;

            int deliverID = -1;
            var delivers = database.Transactions.GroupBy(x => x.DeliveryMan).Where(x => x.Key != null);
            if (delivers.Count() == 0)
                deliverID = database.Users.FirstOrDefault(x => x.UserType == UserType.Delivery).ID;
            else deliverID = delivers.OrderBy(x => x.Count()).First().Key.ID;

            var transaction = database.Transactions.Find(adressModel.Transaction);
            transaction.Adress = adress;
            transaction.TimeDelivery = DateTime.Now.AddDays(adress.Days);
            transaction.IsClose = true;
            transaction.DeliveryMan = database.Users.Find(deliverID);
            database.Update(transaction);
            database.SaveChanges();
            return true;
        }

        public TransactionModel StartTransactionByBasket(int userID)
        {
            DatabaseContextEngine databaseContext = new DatabaseContextEngine();
            var currency = databaseContext.Baskets.Where(x => x.User.ID == userID).Sum(x => x.Count * x.Good.Price);
            var usersBasket = databaseContext.Baskets.Where(x => x.User.ID == userID);
            var transaction = databaseContext.Transactions.FirstOrDefault(x => !x.IsClose);
            var user = databaseContext.Users.Find(userID);
            if (transaction == null)
                transaction = new Transaction
                {
                    User = user,
                    DateTime = DateTime.Now,
                    Currency = currency,
                    IsClose = false,
                };
            List<TransactionGoods> transactionGoods;
            if (transaction.ID != 0)
                transactionGoods = databaseContext.TransactionGoods.Where(x => x.Transaction.ID == transaction.ID).ToList();
            else
                transactionGoods = databaseContext.Baskets.Where(x => x.User.ID == userID).Select(
                    x => new TransactionGoods
                    {
                        Count = x.Count,
                        Good = x.Good.Name,
                        Creator = x.Good.Creator,
                        Transaction = transaction,
                        Price = x.Good.Price
                    }).ToList();
            databaseContext.Transactions.Add(transaction);
            databaseContext.TransactionGoods.AddRange(transactionGoods);
            if (transaction.ID <= 0)
                databaseContext.SaveChanges();
            return new TransactionModel(transaction);
        }

        public List<TransactionModel> Transactions(int id)
        {
            DatabaseContextEngine databaseContextEngine = new DatabaseContextEngine();
            return databaseContextEngine.Transactions.Where(x => x.User.ID == id).Include(x => x.User)
                .Include(x => x.TransactionGoods).Select(x => new TransactionModel(x)).ToList();
        }

        public bool UpdateGood(GoodModel good)
        {
            DatabaseContextEngine databaseContext = new DatabaseContextEngine();
            var findCopy = databaseContext.Goods.Find(good.ActualID);
            findCopy.Price = good.Price;
            findCopy.Name = good.Name;
            findCopy.ImageName = good.ImagePath;
            databaseContext.Update(findCopy);
            databaseContext.SaveChanges();
            return true;
        }

        public List<TransactionModel> DeliveryTransactions(int id)
        {
            DatabaseContextEngine database = new DatabaseContextEngine();
            return database.Transactions.Where(x => x.DeliveryMan.ID == id && !x.IsCancel && !x.IsDeliver)
                .Include(x => x.User).Include(x => x.TransactionGoods).Include(x => x.Adress)
                .Select(x => new TransactionModel(x)).ToList();
        }

        public List<TransactionModel> DeliveryHistory(int id)
        {
            DatabaseContextEngine database = new DatabaseContextEngine();
            return database.Transactions.Where(x => x.DeliveryMan.ID == id && x.IsDeliver)
                .Include(x => x.User).Include(x => x.TransactionGoods).Include(x => x.Adress)
                .Select(x => new TransactionModel(x)).ToList();
        }
    }
}

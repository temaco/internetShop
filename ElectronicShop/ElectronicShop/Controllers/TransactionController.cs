using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopEngine.Models.Input;
using ShopEngine.Models.Output;

namespace ElectronicShop.Controllers
{
    [Authorize(Roles = "User")]
    public class TransactionController : Controller
    {
        ILogger<TransactionController> logger;

        public TransactionController(ILogger<TransactionController> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IActionResult Transaction()
        {
            int user = GetUser();
            ShopEngine.ShopEngineService shopEngine = new ShopEngine.ShopEngineService();
            var transaction = shopEngine.StartTransactionByBasket(user);
            return View(new TransactionInformation
            {
                TransactionID = transaction.ActualID,
                UserID = user
            });
        }

        private int GetUser()
        {
            return int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
        }

        public IActionResult Adress(int id)
        {
            return View(new AdressModel { Transaction = id });
        }

        [HttpPost]
        public IActionResult Adress(AdressModel adressModel)
        {
            if (ModelState.IsValid)
            {
                ShopEngine.ShopEngineService shopEngineService = new ShopEngine.ShopEngineService();
                var result = shopEngineService.SetAdress(adressModel);
                if (!result)
                {
                    ViewBag.Error = "Incorrect adress";
                    return View(adressModel);
                }
                return RedirectToAction("Index", "Home");

            }
            return View(adressModel);
        }

        [HttpPost]
        public IActionResult Transaction(TransactionInformation transactionModel)
        {
            ShopEngine.ShopEngineService shopEngine = new ShopEngine.ShopEngineService();
            shopEngine.TransactionInformation(transactionModel);
            return RedirectToAction("Adress", new { id = transactionModel.TransactionID });
        }
    }
}
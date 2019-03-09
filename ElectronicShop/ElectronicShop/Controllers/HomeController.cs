using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ElectronicShop.Models;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ElectronicShop.Controllers
{
    public class HomeController : Controller
    {
        private int GetUserID()
        {
            return int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
        }

        private ILogger<HomeController> logger;

        public HomeController(ILogger<HomeController> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IActionResult Index()
        {
            ShopEngine.ShopEngineService shopEngineService = new ShopEngine.ShopEngineService();
            return View(shopEngineService.GetGoods());
        }

        public IActionResult GoodOpen(int id)
        {
            logger.LogInformation($"Open good with id = {id}");
            ShopEngine.ShopEngineService shopEngine = new ShopEngine.ShopEngineService();
            return View(shopEngine.GetGoodByID(id));
        }

        [Authorize(Roles = "User")]
        public IActionResult Basket(int id)
        {
            logger.LogInformation($"Open basket for user = {id}");
            ShopEngine.ShopEngineService shopEngine = new ShopEngine.ShopEngineService();
            return View(shopEngine.GetBaskets(id));
        }

        [Authorize(Roles = "User")]
        public IActionResult AddToBasket(int id, [FromForm]int count)
        {
            logger.LogInformation($"Add good with id = {id} * count = {count}");
            ShopEngine.ShopEngineService shopEngine = new ShopEngine.ShopEngineService();
            shopEngine.AddGoodToBasket(id, GetUserID(), count);
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "User")]
        public IActionResult Delete(int id)
        {
            var user = GetUserID();
            logger.LogInformation($"Delete from basket for user = {user}");
            ShopEngine.ShopEngineService shopEngine = new ShopEngine.ShopEngineService();
            shopEngine.DeleteFromBasket(user, id);
            return RedirectToAction("Basket", new { id = user });
        }

        [Authorize(Roles = "User")]
        public IActionResult CancelTransaction(int id)
        {
            ShopEngine.ShopEngineService shopEngineService = new ShopEngine.ShopEngineService();
            shopEngineService.CancelTransactions(id);
            return RedirectToAction("Transactions");
        }

        [Authorize(Roles = "User")]
        public IActionResult Delivery(int id, bool success)
        {
            ShopEngine.ShopEngineService shopEngine = new ShopEngine.ShopEngineService();
            shopEngine.DeliveryUser(id, success);
            return RedirectToAction("Transactions");
        }

        [Authorize(Roles = "User")]
        public IActionResult Transactions()
        {
            ShopEngine.ShopEngineService shopEngine = new ShopEngine.ShopEngineService();
            return View(shopEngine.Transactions(GetUserID()));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

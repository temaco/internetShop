using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicShop.Controllers
{
    [Authorize(Roles = "Delivery")]
    public class DeliveryController : Controller
    {

        public int UserID => int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
        public IActionResult Index()
        {
            ShopEngine.ShopEngineService shopEngine = new ShopEngine.ShopEngineService();
            return View(shopEngine.DeliveryTransactions(UserID));
        }

        public IActionResult Delivery([FromRoute]int id)
        {
            ShopEngine.ShopEngineService shopEngine = new ShopEngine.ShopEngineService();
            shopEngine.DeliverySuccess(id);
            return RedirectToAction("Index", "Delivery");
        }

        public IActionResult History()
        {
            ShopEngine.ShopEngineService shopEngine = new ShopEngine.ShopEngineService();
            return View(shopEngine.DeliveryHistory(UserID));
        }
    }
}
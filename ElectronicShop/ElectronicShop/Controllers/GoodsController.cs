using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ElectronicShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopEngine.Models.Output;

namespace ElectronicShop.Controllers
{
    [Authorize(Roles = "Provider")]
    public class GoodsController : Controller
    {

        ILogger<GoodsController> logger;

        public GoodsController(ILogger<GoodsController> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        private int GetUserID()
        {
            return int.Parse(User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value);
        }

        private string GetUserLogin()
        {
            return User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value;
        }


        public IActionResult Index()
        {
            ShopEngine.ShopEngineService shopEngine = new ShopEngine.ShopEngineService();
            return View(shopEngine.GetGoodsForCreator(GetUserID()));
        }

        public IActionResult GoodNew()
        {
            var model = new GoodWorker { Action = "New", GoodModel = new GoodModel() };
            model.GoodModel.CreatorID = GetUserID();
            return View("Good", model);
        }

        [HttpPost]
        public IActionResult GoodEdit(int id)
        {
            ShopEngine.ShopEngineService shopEngine = new ShopEngine.ShopEngineService();
            return View("Good", new GoodWorker { Action = "Edit", GoodModel = shopEngine.GetGoodByID(id) });
        }

        [HttpPost]
        public IActionResult DeleteGood(int id)
        {
            ShopEngine.ShopEngineService shopEngine = new ShopEngine.ShopEngineService();
            shopEngine.DeleteGood(id);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Good(GoodWorker goodWorker)
        {
            ShopEngine.ShopEngineService shopEngine = new ShopEngine.ShopEngineService();
            var file = Request.Form.Files.FirstOrDefault();
            string path = string.Empty;
            if (file != null && file.Length > 0)
            {
                path = $"images\\{GetUserLogin()}\\".Replace(".", "_").Replace("@", "_");
                if (!string.IsNullOrEmpty(goodWorker.GoodModel.ImagePath))
                    System.IO.File.Delete($"wwwroot\\{goodWorker.GoodModel.ImagePath}");
                var realPath = $"wwwroot\\{path}";
                if (!Directory.Exists(realPath))
                    Directory.CreateDirectory(realPath);
                realPath += file.FileName;
                path += file.FileName;
                if (!System.IO.File.Exists(realPath))
                    using (var stream = new FileStream(realPath, FileMode.OpenOrCreate))
                        file.CopyToAsync(stream);
            }

            goodWorker.GoodModel.ImagePath = path;

            switch (goodWorker.Action.ToLower())
            {
                case "new":
                    shopEngine.AddGood(goodWorker.GoodModel);
                    break;
                case "edit":
                    shopEngine.UpdateGood(goodWorker.GoodModel);
                    break;
            }
            return RedirectToAction("Index");
        }
    }
}
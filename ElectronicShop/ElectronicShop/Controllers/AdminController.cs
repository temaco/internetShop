using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopEngine.Models.Database;

namespace ElectronicShop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public IActionResult UserChangeType()
        {
            return View(new AccountService.AccountService().GetUserModels());
        }

        [HttpPost]
        public IActionResult ChangeType(User user)
        {
            AccountService.AccountService accountService = new AccountService.AccountService();
            accountService.ChangeRole(user.ID, user.UserType);
            return Redirect("UserChangeType");
        }
    }
}
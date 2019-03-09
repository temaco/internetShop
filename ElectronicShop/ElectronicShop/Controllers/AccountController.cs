using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AccountService.Models.Input;
using AccountService.Models.Output;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopEngine.Models.Database;

namespace ElectronicShop.Controllers
{

    public enum UserType
    {
        Admin,
        Provider,
        User
    }

    public class AccountController : Controller
    {

        private ILogger<AccountController> logger;

        public AccountController(ILogger<AccountController> logger)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }


        private async void Login(LoginServiceResult loginModel)
        {
            var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, loginModel.Login),
                            new Claim(ClaimTypes.Role, loginModel.UserType.ToString()),
                            new Claim(ClaimTypes.NameIdentifier, loginModel.ActualID.ToString())
                        };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "ChatFurieLogin");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
            await HttpContext.SignInAsync(claimsPrincipal);
        }

        [HttpPost]
        public IActionResult Login(LoginInputModels loginModel)
        {
            if (ModelState.IsValid)
            {
                AccountService.AccountService authService = new AccountService.AccountService();
                var resultCode = authService.Login(loginModel) as LoginServiceResult;
                if (resultCode.OperationCode == OperationCode.Success)
                {
                    logger.LogInformation($"Log {resultCode.Login} {resultCode.UserType.ToString()}");
                    Login(resultCode);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(loginModel);
        }

        [HttpPost]
        public IActionResult Register(RegisterInputModel registerModel)
        {
            if (ModelState.IsValid)
            {
                registerModel.UserType = ShopEngine.Models.Database.UserType.User;
                AccountService.AccountService authService = new AccountService.AccountService();
                var resultCode = authService.Register(registerModel) as LoginServiceResult;
                if (resultCode.OperationCode == OperationCode.Success)
                {
                    logger.LogInformation($"Register {resultCode.Login} {resultCode.UserType.ToString()}");
                    Directory.CreateDirectory($"wwwroot\\images\\{registerModel.Login.Replace(".", "_").Replace("@", "_")}");
                    Login(resultCode);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(registerModel);
        }

        [Authorize]
        public async Task<IActionResult> SignOut()
        {
            logger.LogInformation(string.Join(", ", User.Claims.Select(x => x.Value).ToArray()));
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
using ShopEngine.Models.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccountService.Models.Input
{
    public class RegisterInputModel
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Login is required")]
        [DataType(DataType.EmailAddress, ErrorMessage = "This is not email format")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Min Length is 6")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirmed Password is required")]
        [MinLength(6, ErrorMessage = "Min Length is 6")]
        public string PasswordHash { get; set; }
        public UserType UserType { get; set; } = UserType.User;
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ShopEngine.Models.Database
{

    public enum UserType
    {
        Admin,
        Provider,
        User,
        Delivery,
        Default
    }

    public class User
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public UserType UserType { get; set; }
    }
}
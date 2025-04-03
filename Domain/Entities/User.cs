using Domain.Common;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        // نقش کاربر (مشتری / مدیر)
        public string Role { get; set; } = "Customer";

        // ارتباط با سفارشات و آدرس
        public List<Order> Orders { get; set; } = new();
        public Address Address { get; set; } = null!;
    }
}

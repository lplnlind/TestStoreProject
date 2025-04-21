using Domain.Common;
using Domain.Enums;
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
        public string UserName { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public Email Email { get; set; } 
        public string PasswordHash { get; set; } = string.Empty;

        // نقش کاربر (مشتری / مدیر)
        public UserRole Role { get; set; }

        // ارتباط با سفارشات و آدرس
        public List<Order> Orders { get; set; } = new();
        public Address Address { get; set; } = null!;
    }
}

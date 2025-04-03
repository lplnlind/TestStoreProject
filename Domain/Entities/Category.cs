﻿using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        // ارتباط یک به چند با محصولات
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Users
{
    public class Permission
    {
        public int Id { get; set; }

        // نام Area (اگر وجود دارد)
        public string? Area { get; set; }

        // نام کنترلر
        public string Controller { get; set; } = string.Empty;

        // نام اکشن
        public string Action { get; set; } = string.Empty;

        // توضیح دسترسی برای خوانایی بهتر
        public string Description { get; set; } = string.Empty;
    }
}

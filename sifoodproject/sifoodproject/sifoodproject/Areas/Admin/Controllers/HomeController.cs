﻿using Microsoft.AspNetCore.Mvc;
using sifoodproject.Areas.Admin.Models;
using sifoodproject.Models;
using System.Security.Cryptography;
using System.Text;

namespace sifoodproject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly Sifood3Context _context;
        public HomeController(Sifood3Context context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginVM model)
        {
            var admin = _context.Admins.Where(x => x.Account == model.Account).FirstOrDefault();
            if (admin != null)
            {
                string passwordWithSalt = $"{model.Password}{admin.PasswordSalt}";
                Byte[] realPasswordBytes = Encoding.ASCII.GetBytes(passwordWithSalt);
                Byte[] realPasswordHash = SHA256.HashData(realPasswordBytes);
                if (Enumerable.SequenceEqual(realPasswordHash, admin.Password))
                {
                    return RedirectToAction("Index", "OrderManage");
                }
            }
            return View();
        }
    }
}

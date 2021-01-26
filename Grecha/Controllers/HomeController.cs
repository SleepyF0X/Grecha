﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DAL.DbContext;

namespace Grecha.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Home
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.ToListAsync();
            return View(products);
        }
        
        public async Task<IActionResult> GetProducts(string storeName)
        {
            var products = await _context.Products.Where(e => e.Shop.ToLower().Equals(storeName.ToLower())).ToListAsync();
            return View("Index", products);
        }
    }
}

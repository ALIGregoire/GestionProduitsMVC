using Microsoft.AspNetCore.Mvc;
using GestionProduitsMVC.Models;
using System.Collections.Generic;

namespace GestionProduitsMVC.Controllers
{
    public class ProductController : Controller
    {
        private static List<Product> _products = new List<Product>
        {
            new Product { Id = 1, Name = "Ordinateur", Price = 250000M, Stock = 5 },
            new Product { Id = 2, Name = "Téléphone", Price = 150000M, Stock = 10 }
        };

        public IActionResult Index()
        {
            return View(_products);
        }

        public IActionResult Details(int id)
        {
            var product = _products.Find(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}

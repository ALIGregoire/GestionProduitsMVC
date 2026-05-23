using Microsoft.AspNetCore.Mvc;
using GestionProduitsMVC.Data;
using Microsoft.EntityFrameworkCore;

namespace GestionProduitsMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalProduits = await _context.Produits.Where(p => !p.IsDeleted).CountAsync();
            ViewBag.TotalClients = await _context.Clients.Where(c => !c.IsDeleted).CountAsync();
            ViewBag.TotalFournisseurs = await _context.Fournisseurs.Where(f => !f.IsDeleted).CountAsync();
            ViewBag.StockCritique = await _context.Produits.Where(p => !p.IsDeleted && p.StockDisponible <= 5).CountAsync();

            return View();
        }
    }
}
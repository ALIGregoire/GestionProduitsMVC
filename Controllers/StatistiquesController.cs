using GestionProduitsMVC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GestionProduitsMVC.Controllers
{
    public class StatistiquesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatistiquesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Statistiques globales
            ViewBag.TotalProduits = await _context.Produits.CountAsync(p => !p.IsDeleted);
            ViewBag.TotalClients = await _context.Clients.CountAsync(c => !c.IsDeleted);
            ViewBag.TotalFournisseurs = await _context.Fournisseurs.CountAsync(f => !f.IsDeleted);
            ViewBag.TotalCommandes = await _context.Commandes.CountAsync(cmd => !cmd.IsDeleted);

            // Stock critique
            ViewBag.StockCritique = await _context.Produits
                .Where(p => !p.IsDeleted && p.StockDisponible <= 5)
                .ToListAsync();

            // Top 5 produits les plus stockés
            ViewBag.TopProduits = await _context.Produits
                .Where(p => !p.IsDeleted)
                .OrderByDescending(p => p.StockDisponible)
                .Take(5)
                .ToListAsync();

            // Produits par catégorie
            ViewBag.ProduitsParCategorie = await _context.Categories
                .Where(c => !c.IsDeleted)
                .Select(c => new { Categorie = c.Nom, Total = c.Produits.Count(p => !p.IsDeleted) })
                .ToListAsync();

            return View();
        }

        // API pour les graphiques (JSON)
        [HttpGet]
        public async Task<IActionResult> GetChartData()
        {
            var data = await _context.Categories
                .Where(c => !c.IsDeleted)
                .Select(c => new { c.Nom, Total = c.Produits.Count(p => !p.IsDeleted) })
                .ToListAsync();

            return Json(data);
        }
    }
}
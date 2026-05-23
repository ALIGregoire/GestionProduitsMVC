using GestionProduitsMVC.Data;
using GestionProduitsMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GestionProduitsMVC.Controllers
{
    public class ProduitController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProduitController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Produit/Index - Liste des produits
        public async Task<IActionResult> Index(string searchString, int? categorieId)
        {
            var produits = _context.Produits
                .Include(p => p.Categorie)
                .Include(p => p.Fournisseur)
                .Where(p => !p.IsDeleted);

            // Filtre par recherche
            if (!string.IsNullOrEmpty(searchString))
            {
                produits = produits.Where(p => p.Libelle.Contains(searchString));
            }

            // Filtre par catégorie
            if (categorieId.HasValue && categorieId > 0)
            {
                produits = produits.Where(p => p.CategorieId == categorieId);
            }

            // ViewBag pour les filtres
            ViewBag.Categories = new SelectList(await _context.Categories.Where(c => !c.IsDeleted).ToListAsync(), "Id", "Nom");
            ViewBag.SearchString = searchString;
            ViewBag.SelectedCategorie = categorieId;

            return View(await produits.ToListAsync());
        }

        // GET: Produit/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produit = await _context.Produits
                .Include(p => p.Categorie)
                .Include(p => p.Fournisseur)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (produit == null)
            {
                return NotFound();
            }

            return View(produit);
        }

        // GET: Produit/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(await _context.Categories.Where(c => !c.IsDeleted).ToListAsync(), "Id", "Nom");
            ViewBag.Fournisseurs = new SelectList(await _context.Fournisseurs.Where(f => !f.IsDeleted).ToListAsync(), "Id", "NomEntreprise");
            return View();
        }

        // POST: Produit/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Libelle,PrixUnitaire,StockDisponible,CategorieId,FournisseurId")] Produit produit)
        {
            if (ModelState.IsValid)
            {
                _context.Add(produit);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Produit ajouté avec succès!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(await _context.Categories.Where(c => !c.IsDeleted).ToListAsync(), "Id", "Nom");
            ViewBag.Fournisseurs = new SelectList(await _context.Fournisseurs.Where(f => !f.IsDeleted).ToListAsync(), "Id", "NomEntreprise");
            return View(produit);
        }

        // GET: Produit/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produit = await _context.Produits.FindAsync(id);
            if (produit == null)
            {
                return NotFound();
            }

            ViewBag.Categories = new SelectList(await _context.Categories.Where(c => !c.IsDeleted).ToListAsync(), "Id", "Nom", produit.CategorieId);
            ViewBag.Fournisseurs = new SelectList(await _context.Fournisseurs.Where(f => !f.IsDeleted).ToListAsync(), "Id", "NomEntreprise", produit.FournisseurId);
            return View(produit);
        }

        // POST: Produit/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Libelle,PrixUnitaire,StockDisponible,CategorieId,FournisseurId,IsDeleted,DateCreation,DateModification")] Produit produit)
        {
            if (id != produit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(produit);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Produit modifié avec succès!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProduitExists(produit.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(await _context.Categories.Where(c => !c.IsDeleted).ToListAsync(), "Id", "Nom", produit.CategorieId);
            ViewBag.Fournisseurs = new SelectList(await _context.Fournisseurs.Where(f => !f.IsDeleted).ToListAsync(), "Id", "NomEntreprise", produit.FournisseurId);
            return View(produit);
        }

        // GET: Produit/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produit = await _context.Produits
                .Include(p => p.Categorie)
                .Include(p => p.Fournisseur)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (produit == null)
            {
                return NotFound();
            }

            return View(produit);
        }

        // POST: Produit/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var produit = await _context.Produits.FindAsync(id);
            if (produit != null)
            {
                // Soft delete via BaseEntity
                _context.Produits.Remove(produit);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Produit archivé avec succès!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProduitExists(int id)
        {
            return _context.Produits.Any(e => e.Id == id && !e.IsDeleted);
        }
    }
}
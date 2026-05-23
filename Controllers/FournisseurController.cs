using GestionProduitsMVC.Data;
using GestionProduitsMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace GestionProduitsMVC.Controllers
{
    public class FournisseurController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FournisseurController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Fournisseur/Index
        public async Task<IActionResult> Index()
        {
            var fournisseurs = await _context.Fournisseurs
                .Include(f => f.Produits)
                .Where(f => !f.IsDeleted)
                .ToListAsync();
            return View(fournisseurs);
        }

        // GET: Fournisseur/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Fournisseur/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NomEntreprise,Email,Telephone")] Fournisseur fournisseur)
        {
            if (ModelState.IsValid)
            {
                _context.Add(fournisseur);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Fournisseur ajouté avec succès!";
                return RedirectToAction(nameof(Index));
            }
            return View(fournisseur);
        }

        // GET: Fournisseur/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fournisseur = await _context.Fournisseurs.FindAsync(id);
            if (fournisseur == null || fournisseur.IsDeleted)
            {
                return NotFound();
            }

            
            return View(fournisseur);
        }

        // POST: Fournisseur/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NomEntreprise,Email,Telephone,IsDeleted,DateCreation,DateModification")] Fournisseur fournisseur)
        {
            if (id != fournisseur.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fournisseur);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Fournisseur modifié avec succès!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FournisseurExists(fournisseur.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return View(fournisseur);
        }

        // GET: Fournisseur/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fournisseur = await _context.Fournisseurs
                .Include(f => f.Produits)
                .FirstOrDefaultAsync(f => f.Id == id && !f.IsDeleted);

            if (fournisseur == null)
            {
                return NotFound();
            }

            // Vérifier si le fournisseur fournit des produits
            if (fournisseur.Produits.Any(p => !p.IsDeleted))
            {
                TempData["Error"] = "Impossible de supprimer ce fournisseur car il est associé à des produits.";
                return RedirectToAction(nameof(Index));
            }

            return View(fournisseur);
        }

        // POST: Fournisseur/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var fournisseur = await _context.Fournisseurs.FindAsync(id);
            if (fournisseur != null)
            {
                _context.Fournisseurs.Remove(fournisseur);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Fournisseur supprimé avec succès!";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool FournisseurExists(int id)
        {
            return _context.Fournisseurs.Any(e => e.Id == id && !e.IsDeleted);
        }
    }
}
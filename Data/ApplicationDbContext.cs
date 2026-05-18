using GestionProduitsMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionProduitsMVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Tables principales
        public DbSet<Produit> Produits { get; set; }
        public DbSet<Categorie> Categories { get; set; }
        public DbSet<Fournisseur> Fournisseurs { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Commande> Commandes { get; set; }
        public DbSet<Livraison> Livraisons { get; set; }
        public DbSet<Facture> Factures { get; set; }

        // Tables de détails (Lignes d'association)
        public DbSet<CommandeDetail> CommandeDetails { get; set; }
        public DbSet<LivraisonDetail> LivraisonDetails { get; set; }
        public DbSet<FactureDetail> FactureDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =================================================================
            // 1. SÉCURITÉ & OPÉRATIONS FINANCIÈRES : Clés Primaires Composites
            // =================================================================
            // Empêche les doublons de produits dans une même commande/facture
            
            modelBuilder.Entity<CommandeDetail>()
                .HasKey(cd => new { cd.CommandeId, cd.ProduitId });

            modelBuilder.Entity<LivraisonDetail>()
                .HasKey(ld => new { ld.LivraisonId, ld.ProduitId });

            modelBuilder.Entity<FactureDetail>()
                .HasKey(fd => new { fd.FactureId, fd.ProduitId });

            // =================================================================
            // 2. CONFIGURATION DES RELATIONS & PROTECTION DES DONNÉES (Restric)
            // =================================================================

            // Un Produit ne peut pas être supprimé s'il est lié à une Catégorie active
            modelBuilder.Entity<Produit>()
                .HasOne(p => p.Categorie)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategorieId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Produit>()
                .HasOne(p => p.Fournisseur)
                .WithMany(f => f.Products)
                .HasForeignKey(p => p.FournisseurId)
                .OnDelete(DeleteBehavior.Restrict);

            // Un Produit ne peut pas être supprimé s'il a un historique d'achats/ventes
            modelBuilder.Entity<CommandeDetail>()
                .HasOne(cd => cd.Produit)
                .WithMany()
                .HasForeignKey(cd => cd.ProduitId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<LivraisonDetail>()
                .HasOne(ld => ld.Produit)
                .WithMany()
                .HasForeignKey(ld => ld.ProduitId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FactureDetail>()
                .HasOne(fd => fd.Produit)
                .WithMany()
                .HasForeignKey(fd => fd.ProduitId)
                .OnDelete(DeleteBehavior.Restrict);

            // Interdiction absolue de supprimer un client si des commandes ou factures existent
            modelBuilder.Entity<Commande>()
                .HasOne(c => c.Client)
                .WithMany() // ou .WithMany(cl => cl.Commandes) si la liste existe dans Client
                .HasForeignKey(c => c.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}

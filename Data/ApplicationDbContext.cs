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

            // 1. Clés composites automatiques pour les tables de détails
            modelBuilder.Entity<CommandeDetail>().HasKey(cd => new { cd.CommandeId, cd.ProduitId });
            modelBuilder.Entity<LivraisonDetail>().HasKey(ld => new { ld.LivraisonId, ld.ProduitId });
            modelBuilder.Entity<FactureDetail>().HasKey(fd => new { fd.FactureId, fd.ProduitId });

            // 2. FILTRE GLOBAL DE SÉCURITÉ : Bloque l'accès aux données marquées comme supprimées logiquement (Soft Delete)
            modelBuilder.Entity<Categorie>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Fournisseur>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Client>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Produit>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Commande>().HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Facture>().HasQueryFilter(e => !e.IsDeleted);

            // 3. Protection stricte contre les suppressions en cascade accidentelles
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        // 4. Interception des sauvegardes pour gérer automatiquement les dates d'audit et le Soft Delete
        public override int SaveChanges()
        {
            ProcessAuditAndSoftDelete();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            ProcessAuditAndSoftDelete();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void ProcessAuditAndSoftDelete()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is BaseEntity entity)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entity.DateCreation = DateTime.UtcNow;
                            entity.IsDeleted = false;
                            break;
                        case EntityState.Modified:
                            entity.DateModification = DateTime.UtcNow;
                            break;
                        case EntityState.Deleted:
                            // Intercepte la vraie suppression physique pour la transformer en archivage logique
                            entry.State = EntityState.Modified;
                            entity.IsDeleted = true;
                            entity.DateModification = DateTime.UtcNow;
                            break;
                    }
                }
            }
        }

    }
}

namespace GestionProduitsMVC.Models
{
    // Interface pour identifier les entités qui supportent l'archivage sans suppression physique
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }

    // Classe de base pour la traçabilité (Audit Trail)
    public abstract class BaseEntity : ISoftDelete
    {
        public bool IsDeleted { get; set; } = false;
        public DateTime DateCreation { get; set; } = DateTime.UtcNow;
        public DateTime? DateModification { get; set; }
    }
}

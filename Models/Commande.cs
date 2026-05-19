using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionProduitsMVC.Models
{
    public class Commande : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DateCommande { get; set; } = DateTime.UtcNow;

        [Required]
        [StringLength(50)]
        public string Statut { get; set; } = "EnAttente"; // EnAttente, Livree, Annulee

        [Required]
        public int ClientId { get; set; }
        [ForeignKey(nameof(ClientId))]
        public virtual Client? Client { get; set; }

        public virtual ICollection<CommandeDetail> Details { get; set; } = new List<CommandeDetail>();
    }

    public class CommandeDetail
    {
        [Required]
        public int CommandeId { get; set; }
        [ForeignKey(nameof(CommandeId))]
        public virtual Commande? Commande { get; set; }

        [Required]
        public int ProduitId { get; set; }
        [ForeignKey(nameof(ProduitId))]
        public virtual Produit? Produit { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La quantité doit être d'au moins 1.")]
        public int Quantite { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrixApplique { get; set; } // Sécurité financière historique
    }
}

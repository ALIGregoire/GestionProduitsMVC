using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionProduitsMVC.Models
{
    public class Facture : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string NumeroFacture { get; set; } = string.Empty; // Format légal EX: FACT-2026-0001

        [Required]
        public DateTime DateEmission { get; set; } = DateTime.UtcNow;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalHT { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalTVA { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalTTC { get; set; }

        [Required]
        public int CommandeId { get; set; }
        [ForeignKey(nameof(CommandeId))]
        public virtual Commande? Commande { get; set; }

        public virtual ICollection<FactureDetail> Details { get; set; } = new List<FactureDetail>();
    }

    public class FactureDetail
    {
        [Required]
        public int FactureId { get; set; }
        [ForeignKey(nameof(FactureId))]
        public virtual Facture? Facture { get; set; }

        [Required]
        public int ProduitId { get; set; }
        [ForeignKey(nameof(ProduitId))]
        public virtual Produit? Produit { get; set; }

        [Required]
        public int Quantite { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PrixUnitaireFacture { get; set; }
    }
}

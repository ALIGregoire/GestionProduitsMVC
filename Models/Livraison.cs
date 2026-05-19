using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionProduitsMVC.Models
{
    public class Livraison : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime DateExpedition { get; set; }

        [Required]
        [StringLength(100)]
        public string NumeroSuivi { get; set; } = string.Empty;

        public virtual ICollection<LivraisonDetail> Details { get; set; } = new List<LivraisonDetail>();
    }

    public class LivraisonDetail
    {
        [Required]
        public int LivraisonId { get; set; }
        [ForeignKey(nameof(LivraisonId))]
        public virtual Livraison? Livraison { get; set; }

        [Required]
        public int ProduitId { get; set; }
        [ForeignKey(nameof(ProduitId))]
        public virtual Produit? Produit { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int QuantiteLivree { get; set; }
    }
}

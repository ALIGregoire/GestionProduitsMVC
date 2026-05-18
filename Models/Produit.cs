using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionProduitsMVC.Models
{
    public class Produit : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Libelle { get; set; } = string.Empty;

        [Required]
        [Range(0.01, 1000000.00, ErrorMessage = "Le prix doit être strictement supérieur à 0.")]
        [Column(TypeName = "decimal(18,2)")] // Précision monétaire SQL stricte
        public decimal PrixUnitaire { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Le stock ne peut pas être négatif.")]
        public int StockDisponible { get; set; }

        // Clés étrangères et relations
        [Required]
        public int CategorieId { get; set; }
        [ForeignKey(nameof(CategorieId))]
        public virtual Categorie? Categorie { get; set; }

        [Required]
        public int FournisseurId { get; set; }
        [ForeignKey(nameof(FournisseurId))]
        public virtual Fournisseur? Fournisseur { get; set; }

    }
}

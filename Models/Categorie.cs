using System.ComponentModel.DataAnnotations;

namespace GestionProduitsMVC.Models
{
    public class Categorie : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Le nom de la catégorie est obligatoire.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Le nom doit contenir entre {2} et {1} caractères.")]
        public string Nom { get; set; } = string.Empty;

        // Propriété de navigation (Relation 1-N)
        public virtual ICollection<Produit> Produits { get; set; } = new List<Produit>();
    }
}

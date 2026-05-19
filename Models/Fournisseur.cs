using System.ComponentModel.DataAnnotations;

namespace GestionProduitsMVC.Models
{
    public class Fournisseur : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string NomEntreprise { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "Le format de l'adresse email est invalide.")]
        [StringLength(256)]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Le format du numéro de téléphone est invalide.")]
        [StringLength(20)]
        public string Telephone { get; set; } = string.Empty;

        public virtual ICollection<Produit> Produits { get; set; } = new List<Produit>();
    }
}

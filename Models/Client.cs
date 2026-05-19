using System.ComponentModel.DataAnnotations;

namespace GestionProduitsMVC.Models
{
    public class Client : BaseEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nom { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Prenom { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(256)]

        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Le format du numéro de téléphone est invalide.")]
        [StringLength(20)]
        public string Telephone { get; set; } = string.Empty;



        public virtual ICollection<Commande> Commandes { get; set; } = new List<Commande>();
    }
}

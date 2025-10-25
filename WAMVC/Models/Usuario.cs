using System.ComponentModel.DataAnnotations;

namespace WAMVC.Models
{
 public class Usuario
 {
 public int Id { get; set; }

 [Required]
 [StringLength(100)]
 public string Nombre { get; set; }

 [Required]
 [EmailAddress]
 public string Email { get; set; }

 public string PasswordHash { get; set; }

 [StringLength(50)]
 public string Role { get; set; }
 }
}
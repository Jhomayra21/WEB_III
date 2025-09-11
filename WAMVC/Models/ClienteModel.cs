using System.ComponentModel.DataAnnotations;

namespace WAMVC.Models
{
    public class ClienteModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del cliente es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener mas de 100 caracteres")]
        [Display(Name = "Nombre Completo")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo electronico inválido")]
        [StringLength(150, ErrorMessage = "El correo no puede tener mas de 150 caracteres")]
        [Display(Name = "Correo Electronico")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria")]
        [StringLength(250, ErrorMessage = "La dirección no puede tener mas de 250 caracteres")]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        public ICollection<PedidoModel>? Pedidos { get; set; }
    }
}

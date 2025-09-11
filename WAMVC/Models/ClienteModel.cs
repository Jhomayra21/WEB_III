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

        [Required(ErrorMessage = "El correo electr�nico es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo electronico inv�lido")]
        [StringLength(150, ErrorMessage = "El correo no puede tener mas de 150 caracteres")]
        [Display(Name = "Correo Electronico")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La direcci�n es obligatoria")]
        [StringLength(250, ErrorMessage = "La direcci�n no puede tener mas de 250 caracteres")]
        [Display(Name = "Direcci�n")]
        public string Direccion { get; set; }

        public ICollection<PedidoModel>? Pedidos { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WAMVC.Models
{
    public class PedidoModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La fecha del pedido es obligatoria")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha del Pedido")]
        public DateTime FechaPedido { get; set; }

        [Required(ErrorMessage = "El cliente es obligatorio")]
        [Display(Name = "Cliente")]
        public int IdCliente { get; set; }

        [Required(ErrorMessage = "La dirección de entrega es obligatoria")]
        [StringLength(250, ErrorMessage = "La dirección no puede tener más de 250 caracteres")]
        [Display(Name = "Dirección de Entrega")]
        public string Direccion { get; set; }

        [Required(ErrorMessage = "El monto total es obligatorio")]
        [Range(0.01, 9999999.99, ErrorMessage = "El monto debe ser mayor a 0")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Total")]
        public decimal MontoTotal { get; set; }

        public ClienteModel? Cliente { get; set; }
        public ICollection<DetallePedidoModel>? DetallePedidos { get; set; }
    }
}

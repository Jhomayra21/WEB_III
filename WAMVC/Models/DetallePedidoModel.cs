using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WAMVC.Models
{
    public class DetallePedidoModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El pedido es obligatorio")]
        [Display(Name = "Pedido")]
        public int IdPedido { get; set; }

        [Required(ErrorMessage = "El producto es obligatorio")]
        [Display(Name = "Producto")]
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria")]
        [Range(1, 1000, ErrorMessage = "La cantidad debe estar entre 1 y 1000 unidades")]
        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El precio unitario es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Precio Unitario")]
        public decimal PrecioUnitario { get; set; }

        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 2)")]
        [Display(Name = "Subtotal")]
        public decimal Subtotal { get { return Cantidad * PrecioUnitario; } }
        public PedidoModel? Pedido { get; set; }
        public ProductoModel? Producto { get; set; }
    }
}

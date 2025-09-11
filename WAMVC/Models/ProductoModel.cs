using System.ComponentModel.DataAnnotations;

namespace WAMVC.Models
{
    public class ProductoModel
    {
        //No se olviden es la llave primaria de la tabla
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El nombre del producto es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede tener más de 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "La descripción no puede tener más de 500 caracteres")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio del producto es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a cero")]
        public decimal Precio { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio.")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }

        // Propiedad de navegación para la relación con DetallePedido.
        // Un producto puede estar en muchos detalles de pedido.
        public ICollection<DetallePedidoModel>? DetallePedidos { get; set; }
    }
}

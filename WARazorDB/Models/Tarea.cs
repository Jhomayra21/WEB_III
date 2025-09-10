using System.ComponentModel.DataAnnotations;

namespace WARazorDB.Models
{
    public class Tarea
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la tarea es obligatorio")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
        [Display(Name = "Nombre de la Tarea")]
        public string nombreTarea { get; set; }

        [Required(ErrorMessage = "La fecha de vencimiento es obligatoria")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Vencimiento")]
        [FutureDate(ErrorMessage = "La fecha de vencimiento debe ser posterior a la fecha actual")]
        public DateTime fechaVencimiento { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio")]
        [Display(Name = "Estado")]
        [RegularExpression("^(Pendiente|En Progreso|Completada)$", 
            ErrorMessage = "El estado debe ser: Pendiente, En Progreso o Completada")]
        public string estado { get; set; }

        [Required(ErrorMessage = "El ID de usuario es obligatorio")]
        [Display(Name = "ID de Usuario")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID de usuario debe ser un número positivo")]
        public int idUsuario { get; set; }
    }
}

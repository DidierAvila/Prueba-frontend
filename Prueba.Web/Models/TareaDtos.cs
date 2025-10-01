using System.ComponentModel.DataAnnotations;

namespace Prueba.Web.Models
{
    public class CreateTareaDto
    {
        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "El título debe tener entre 1 y 200 caracteres")]
        [Display(Name = "Título")]
        public required string Title { get; set; }

        [StringLength(1000, ErrorMessage = "La descripción no puede exceder los 1000 caracteres")]
        [Display(Name = "Descripción")]
        public string? Description { get; set; }

        [Display(Name = "Completada")]
        public bool IsCompleted { get; set; }
    }

    public class UpdateTareaDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "El título debe tener entre 1 y 200 caracteres")]
        [Display(Name = "Título")]
        public required string Title { get; set; }

        [StringLength(1000, ErrorMessage = "La descripción no puede exceder los 1000 caracteres")]
        [Display(Name = "Descripción")]
        public string? Description { get; set; }

        [Display(Name = "Completada")]
        public bool IsCompleted { get; set; }
    }
}
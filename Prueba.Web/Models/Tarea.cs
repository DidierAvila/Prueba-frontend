using System.ComponentModel.DataAnnotations;

namespace Prueba.Web.Models
{
    public class Tarea
    {
        public int Id { get; set; }

        [Display(Name = "Título")]
        public string? Title { get; set; }

        [Display(Name = "Descripción")]
        public string? Description { get; set; }

        [Display(Name = "Completada")]
        public bool IsCompleted { get; set; }
    }
}
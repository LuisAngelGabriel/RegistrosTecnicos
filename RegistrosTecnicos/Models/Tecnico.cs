using System.ComponentModel.DataAnnotations;

namespace RegistrosTecnicos.Models
{
    public class Tecnico
    {
        [Key]
        public int TecnicoId { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "Este campo es requerido")]
        public double SueldoHora { get; set; }
    }
}

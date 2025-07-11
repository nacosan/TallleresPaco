using System.ComponentModel.DataAnnotations;

namespace TallleresPaco.Models
{
    public class UsuarioRegistroViewModel
    {
        [Required]
        [StringLength(50)]
        public string? Nombre { get; set; }

        [Required]
        [StringLength(50)]
        public string? Apellido { get; set; }

        [Required]
        public DateTime? FechaNacimiento { get; set; }

        [Required]
        [StringLength(9)]
        public string? Dni { get; set; }

        public string? Estado { get; set; }
    }

}

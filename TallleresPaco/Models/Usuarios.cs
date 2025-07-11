using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TallleresPaco.Models
{
    [Table("usuarios")]
    public class Usuarios
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("nombre")]
        [StringLength(50)]
        public string? Nombre { get; set; }

        [Column("apellido")]
        [StringLength(50)]
        public string? Apellido { get; set; }

        [Column("fecnac")]

        public DateTime? FechaNacimiento { get; set; }


        [Column("dni")]
        [StringLength(9)]
        public string? Dni { get; set; }

        [Column("email")]
        [StringLength(100)]
        public string? Email { get; set; }

        [Column("estado")]
        [StringLength(50)]
        public string? Estado { get; set; }

        public ICollection<Alquileres>? Alquileres { get; set; }
    }
}

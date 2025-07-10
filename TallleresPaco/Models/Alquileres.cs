using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace T1Alquileres.Models
{
    [Table("alquileres")]
    public class Alquileres
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [ForeignKey("Usuario")]
        [Column("idusu")]
        public int UsuarioId { get; set; }

        [ForeignKey("Vehiculo")]
        [Column("idveh")]
        public int VehiculoId { get; set; }

        [Column("fecini")]
        public DateOnly FechaInicio { get; set; }

        [Column("fecfin")]
        public DateOnly FechaFin { get; set; }

        [Column("precio")]
        public decimal Precio { get; set; }

        [Column("prefin")]
        public decimal PrecioFinal { get; set; }

        [Column("estado")]
        [StringLength(50)]
        public string Estado { get; set; }

        public virtual Usuarios Usuario { get; set; }
        public virtual Vehiculos Vehiculo { get; set; }
    }
}

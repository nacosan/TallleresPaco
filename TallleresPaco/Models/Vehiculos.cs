using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace T1Alquileres.Models
{
    [Table("vehiculos")]
    public class Vehiculos
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("matricula")]
        [StringLength(7)]
        public string Matricula { get; set; }

        [Column("modelo")]
        [StringLength(50)]
        public string Modelo { get; set; }

        [Column("marca")]
        [StringLength(50)]
        public string Marca { get; set; }

        [Column("color")]
        [StringLength(50)]
        public string Color { get; set; }

        [Column("aniofab")]
        public DateOnly AnioFab { get; set; }

        [Column("tipo")]
        [StringLength(50)]
        public string Tipo { get; set; }

        [Column("precio")]
        public decimal Precio { get; set; }

        [Column("categoria")]
        [StringLength(50)]
        public string Categoria { get; set; }

        [Column("estado")]
        [StringLength(50)]
        public string Estado { get; set; }

        public ICollection<Alquileres> Alquileres { get; set; }
    }
}

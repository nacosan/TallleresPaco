namespace TallleresPaco.Models
{
    public class ReportesViewModel
    {
        // Para usuarios registrados últimos 2 meses
        public List<UsuarioReporte> UsuariosRecientes { get; set; }

        // Para histórico de alquileres por usuario
        public List<AlquilerReporte> HistoricoAlquileres { get; set; }

        // Para listado de vehículos (para PDF u otros reportes)
        public List<VehiculoReporte> Vehiculos { get; set; }

        // Para reporte escudería (nombre escudería + cantidad de alquileres)
        public List<EscuderiaReporte> Escuderias { get; set; }

        // Para reservas de usuario
        public List<AlquilerReporte> ReservasUsuario { get; set; }

        // Para reservas últimos 2 meses
        public List<AlquilerReporte> ReservasUltimosDosMeses { get; set; }
    }

    public class UsuarioReporte
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public string Email { get; set; }
    }

    public class AlquilerReporte
    {
        public int IdAlquiler { get; set; }
        public string UsuarioNombre { get; set; }
        public string VehiculoMatricula { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public decimal Precio { get; set; }
        public string Estado { get; set; }
    }

    public class VehiculoReporte
    {
        public int IdVehiculo { get; set; }
        public string Matricula { get; set; }
        public string Modelo { get; set; }
        public string Marca { get; set; }
        public string Color { get; set; }
        public DateTime AnioFab { get; set; }
        public decimal Precio { get; set; }
        public string Categoria { get; set; }
    }

    public class EscuderiaReporte
    {
        public string NombreEscuderia { get; set; }
        public int TotalAlquileres { get; set; }
    }
}

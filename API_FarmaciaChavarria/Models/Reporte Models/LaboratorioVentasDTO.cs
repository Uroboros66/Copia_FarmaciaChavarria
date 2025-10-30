namespace API_FarmaciaChavarria.Models.Reporte_Models
{
    public class LaboratorioVentasDTO
    {
        public int IdLaboratorio { get; set; }
        public string NombreLaboratorio { get; set; } = string.Empty;
        public decimal TotalVentas { get; set; }
    }
}
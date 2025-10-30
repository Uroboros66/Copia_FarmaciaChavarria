namespace API_FarmaciaChavarria.Models.Reporte_Models
{
    public class RevenueDataItem
    {
        public string Date { get; set; } = string.Empty; // Ej: "Ene", "Feb", etc.
        public decimal Revenue { get; set; } // Total de ventas
    }
}
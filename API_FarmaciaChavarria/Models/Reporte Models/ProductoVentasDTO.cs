namespace API_FarmaciaChavarria.Models.Reporte_Models
{
    public class ProductoVentasDTO
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; } = string.Empty;
        public decimal TotalVentas { get; set; }
    }
}
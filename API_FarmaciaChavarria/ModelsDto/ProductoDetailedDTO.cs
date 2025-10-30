namespace API_FarmaciaChavarria.ModelsDto
{
    public class ProductoDetailedDTO
    {
        public int IdProducto { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Id_categoria { get; set; }
        public int Id_laboratorio { get; set; }
        public string CategoriaNombre { get; set; } = string.Empty;
        public string LaboratorioNombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int Stock_Minimo { get; set; }
        public string? Efectos_secundarios { get; set; }
        public string? Como_usar { get; set; }
        public DateOnly FechaVencimiento { get; set; }
    }

}
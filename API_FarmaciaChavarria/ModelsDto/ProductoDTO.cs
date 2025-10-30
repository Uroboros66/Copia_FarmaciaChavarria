namespace API_FarmaciaChavarria.ModelsDto
{
    public class ProductoDTO
    {
        public int Id_producto { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Id_categoria { get; set; }
        public int Id_laboratorio { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int Stock_minimo { get; set; }
        public string Efectos_secundarios { get; set; } = string.Empty;
        public string Como_usar { get; set; } = string.Empty;
        public DateOnly Fecha_vencimiento { get; set; }
    }

}
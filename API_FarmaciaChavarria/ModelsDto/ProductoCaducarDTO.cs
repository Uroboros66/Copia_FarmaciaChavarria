namespace API_FarmaciaChavarria.ModelsDto
{
    public class ProductoCaducarDTO
    {
        public int Id_producto { get; set; }  // Clave primaria y foránea a la vez
        public string Nombre { get; set; } = string.Empty;
        public DateOnly Fecha_vencimiento { get; set; }
    }
}
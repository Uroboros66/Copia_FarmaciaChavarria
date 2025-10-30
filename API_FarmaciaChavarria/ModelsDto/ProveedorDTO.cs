namespace API_FarmaciaChavarria.ModelsDto
{
    public class ProveedorDTO
    {
        public int Id_proveedor { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
    }
}
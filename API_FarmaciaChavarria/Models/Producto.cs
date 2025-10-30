using System.ComponentModel.DataAnnotations;

namespace API_FarmaciaChavarria.Models
{
    public class Producto
    {
        [Key]
        public int Id_producto { get; set; }  // Clave primaria
        public string Nombre { get; set; } = string.Empty;
        public int Id_categoria { get; set; }  // Clave foránea
        public int Id_laboratorio { get; set; }  // Clave foránea
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int Stock_minimo { get; set; }
        public string? Efectos_secundarios { get; set; }
        public string? Como_usar { get; set; }
        public DateOnly Fecha_vencimiento { get; set; }

    }

}
using System.ComponentModel.DataAnnotations;

namespace API_FarmaciaChavarria.Models
{
    public class ProductoCaducar
    {
        [Key]
        public int Id_producto { get; set; }  // Clave primaria y foránea a la vez
        public string Nombre { get; set; } = string.Empty;
        public DateOnly Fecha_vencimiento { get; set; }


    }

}
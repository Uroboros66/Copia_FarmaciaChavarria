using System.ComponentModel.DataAnnotations;

namespace API_FarmaciaChavarria.Models
{
    public class Factura
    {
        [Key]
        public int Id_factura { get; set; }
        public DateTime Fecha_venta { get; set; }
        public decimal Total { get; set; }
        public int Id_usuario { get; set; }
    }
}
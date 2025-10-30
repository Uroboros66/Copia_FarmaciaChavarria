using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace API_FarmaciaChavarria.Models
{
    public class Compra
    {
        [Key]
        public int Id_compra { get; set; }
        public int Id_proveedor { get; set; }
        public DateTime Fecha_compra { get; set; }
        public decimal Total { get; set; }
    }
}
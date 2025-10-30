using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API_FarmaciaChavarria.Models
{
    public class DetalleCompra
    {
        [Key]
        public int Id_detalle { get; set; }
        public int Id_compra { get; set; }
        public int Id_producto { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio_unitario { get; set; }
        [NotMapped]
        public decimal Subtotal { get { return Cantidad * Precio_unitario; } }
    }
}
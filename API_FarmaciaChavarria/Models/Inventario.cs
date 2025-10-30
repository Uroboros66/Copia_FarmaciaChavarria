using System;
using System.ComponentModel.DataAnnotations;

namespace API_FarmaciaChavarria.Models
{
    public class Inventario
    {
        [Key]
        public int Id_movimiento { get; set; }
        public int Id_producto { get; set; }
        public string Tipo_movimiento { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public DateTime Fecha_movimiento { get; set; }
        public int Id_usuario { get; set; }
    }
}
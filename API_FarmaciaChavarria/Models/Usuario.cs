using System.ComponentModel.DataAnnotations;

namespace API_FarmaciaChavarria.Models
{
    public class Usuario
    {
        [Key]
        public int Id_usuario { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public int Pin { get; set; }
        public string Rol { get; set; } = string.Empty;
    }
}
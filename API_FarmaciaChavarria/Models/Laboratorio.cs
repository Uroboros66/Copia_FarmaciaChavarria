using System.ComponentModel.DataAnnotations;

namespace API_FarmaciaChavarria.Models
{
    public class Laboratorio
    {
        [Key]
        public int Id_laboratorio { get; set; }
        public string Nombre { get; set; } = string.Empty;

    }
}
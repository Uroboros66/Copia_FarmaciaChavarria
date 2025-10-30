using System.ComponentModel.DataAnnotations;

namespace API_FarmaciaChavarria.Models
{
    public class Categoria
    {
        [Key]
        public int Id_categoria { get; set; }

        public string Nombre { get; set; } = string.Empty;

    }
}
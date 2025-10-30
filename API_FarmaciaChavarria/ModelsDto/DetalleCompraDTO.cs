namespace API_FarmaciaChavarria.ModelsDto
{
    public class DetalleCompraDTO
    {
        public int Id_compra { get; set; }
        public int Id_producto { get; set; }
        public int Cantidad { get; set; }
        public decimal Precio_unitario { get; set; }
    }
}
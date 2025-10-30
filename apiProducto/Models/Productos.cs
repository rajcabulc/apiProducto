using System.ComponentModel.DataAnnotations;

namespace apiProducto.Models
{
    public class Productos
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Stock {  get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}

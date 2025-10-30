using apiProducto.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace apiProducto.Context
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opciones): base(opciones) { }

        public DbSet<Productos> Productos { get; set; }
    }
}

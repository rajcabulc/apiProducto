
using apiProducto.Context;
using Microsoft.EntityFrameworkCore;

namespace apiProducto
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var cadenaConexion = builder.Configuration.GetConnectionString("ConexionDb");
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(cadenaConexion));

            // Configurar CORS para permitir solicitudes desde el frontend
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("PoliticaDev", app =>
                    app.AllowAnyOrigin() // Permite cualquier origen // WithOrigins("http://localhost:8090") // Origen del frontend.
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Habilitar CORS
            app.UseCors("PoliticaDev");

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

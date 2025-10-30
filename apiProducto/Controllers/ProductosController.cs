using apiProducto.Context;
using apiProducto.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;

namespace apiProducto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly AppDbContext _dbcontext;
        public ProductosController(AppDbContext dbContext)
        {
            _dbcontext = dbContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetProductos()
        {
            try
            {
                var productos = await _dbcontext.Productos.ToListAsync();

                return Ok(productos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener los productos, Error: {ex.Message}");
            }
            
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProducto(int id)
        {
            try
            {
                var producto = await _dbcontext.Productos.FindAsync(id);
                if (producto == null)
                {
                    var error = new { message = $"Producto con Id {id} no encontrado." };
                    return NotFound(error);
                }

                return Ok(producto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener los productos, Error: {ex.Message}");
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> NuevoProducto([FromBody] Productos producto)
        {
            
            try
            {
                if (producto == null)
                {
                    var error = new { message = "No existe informacion en la solicitud" };
                    return BadRequest(error);
                }

                if (string.IsNullOrEmpty(producto.Nombre))
                {
                    var error = new { message = "El campo nombre no puede estar vacio" };
                    return BadRequest(error);
                }

                if (producto.Precio <= 0)
                {
                    var error = new { message = "El Precio debe ser mayor a 0" };
                    return BadRequest(error);
                }

                _dbcontext.Productos.Add(producto);
                await _dbcontext.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al agregar el producto, Error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarProducto(int id, [FromBody] Productos producto)
        {
            if (id != producto.Id)
            {
                var error = new { message = "Los Id no coinciden" };
                return BadRequest(error);
            }
            try
            {
                var productoDb = await _dbcontext.Productos.FindAsync(id);
                if (productoDb == null)
                {
                    var error = new { message = $"El producto con el Id {id} no Existe" };
                    return NotFound(error);
                }

                if (string.IsNullOrWhiteSpace(producto.Nombre))
                {
                    var error = new { message = "El campo nombre no puede estar vacio" };
                    return BadRequest(error);
                }

                if (producto.Precio <= 0)
                {
                    var error = new { message = "El Precio debe ser mayor a 0" };
                    return BadRequest(error);
                }

                _dbcontext.Entry(productoDb).CurrentValues.SetValues(producto);
                await _dbcontext.SaveChangesAsync();

                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                var error = new { message = $"Producto con Id {id} no encontrado." };
                return NotFound(error);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al Actualizar producto, Error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            try
            {
                var producto = await _dbcontext.Productos.FindAsync(id);
                if (producto == null)
                {
                    var error = new { message = $"Producto con Id {id} no existe." };
                    return NotFound(error);
                }

                _dbcontext.Productos.Remove(producto);
                await _dbcontext.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al Eliminar producto, Error: {ex.Message}");
            }
        }
    }
}

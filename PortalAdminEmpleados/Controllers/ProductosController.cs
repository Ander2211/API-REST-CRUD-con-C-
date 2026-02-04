using Microsoft.AspNetCore.Mvc;
using PortalAdminEmpleados.Data;
using PortalAdminEmpleados.Modelos;
using PortalAdminEmpleados.Modelos.Entidades;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PortalAdminEmpleados.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly Conexion dbContext; 

        public ProductosController(Conexion dbContext)
        {
            this.dbContext = dbContext;
        }


        // GET: api/<ProductosController>
        [HttpGet]
        public IActionResult GetProductos()
        {
            return Ok(dbContext.Productos.ToList());
        }

        // GET api/<ProductosController>/5
        [HttpGet("{id}")]
        public IActionResult GetByIdProducto(int id)
        {
            var producto = dbContext.Productos.Find(id);
            if (producto == null)
            {
                return NotFound();
            }
            return Ok(producto);
        }

        // POST api/<ProductosController>
        [HttpPost]
        public IActionResult AddProducto(ProductoDTO productoDTO)
        {
            var producto = new Productos()
            {
                Id = productoDTO.Id,
                Nombre = productoDTO.Nombre,
                Descripcion = productoDTO.Descripcion,
                Precio = productoDTO.Precio,
                Stock = productoDTO.Stock,
                CategoriaId = productoDTO.CategoriaId
            };

            dbContext.Productos.Add(producto);
            dbContext.SaveChanges();

            return Ok(producto);
        }

        // PUT api/<ProductosController>/5
        [HttpPut("{id}")]
        public IActionResult UpdateProducto(int id,  ProductoDTO productoDTO)
        {
            if (id != productoDTO.Id)
            {
                return BadRequest("No se encontro el producto");
            }

            var producto = dbContext.Productos.Find(id);
            if (producto == null)
            {
                return NotFound();
            }


            producto.Nombre = productoDTO.Nombre;
            producto.Descripcion = productoDTO.Descripcion;
            producto.Precio = productoDTO.Precio;
            producto.Stock = productoDTO.Stock;
            producto.CategoriaId = productoDTO.CategoriaId;
            dbContext.SaveChanges();

            return Ok(producto);
        }

        // DELETE api/<ProductosController>/5
        [HttpDelete("{id}")]
        public IActionResult DeleteProducto(int id)
        {
            var producto = dbContext.Productos.Find(id);
            if (producto == null)
            {
                return NotFound();
            }
            dbContext.Productos.Remove(producto);
            dbContext.SaveChanges();

            return Ok(producto);
        }
    }
}

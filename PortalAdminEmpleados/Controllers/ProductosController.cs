using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortalAdminEmpleados.Data;
using PortalAdminEmpleados.Modelos;
using PortalAdminEmpleados.Modelos.Entidades;

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

        // GET: api/productos?page=1&pageSize=50
        [HttpGet]
        public async Task<IActionResult> GetProductos(int page = 1, int pageSize = 50, CancellationToken cancellationToken = default)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 500);

            var query = dbContext.Productos
                .AsNoTracking()
                .OrderBy(p => p.Id);

            var total = await query.CountAsync(cancellationToken);
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return Ok(new { total, page, pageSize, items });
        }

        // GET api/productos/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdProducto(int id, CancellationToken cancellationToken = default)
        {
            var producto = await dbContext.Productos
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

            if (producto == null) return NotFound();
            return Ok(producto);
        }

        // POST api/productos
        [HttpPost]
        public async Task<IActionResult> AddProducto(ProductoDTO productoDTO, CancellationToken cancellationToken = default)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // Validar categoría existente para evitar violación de FK
            var categoriaExists = await dbContext.Categorias
                .AnyAsync(c => c.Id == productoDTO.CategoriaId, cancellationToken);
            if (!categoriaExists) return BadRequest("La categoría indicada no existe.");

            var producto = new Productos()
            {
                // No asignar Id manualmente si la BD usa identidad/autoincrement
                Nombre = productoDTO.Nombre,
                Descripcion = productoDTO.Descripcion,
                Precio = productoDTO.Precio,
                Stock = productoDTO.Stock,
                CategoriaId = productoDTO.CategoriaId
            };

            await dbContext.Productos.AddAsync(producto, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            return Ok(producto);
        }

        // PUT api/productos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProducto(int id, ProductoDTO productoDTO, CancellationToken cancellationToken = default)
        {
            if (id != productoDTO.Id) return BadRequest("Id del recurso y payload no coinciden.");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var producto = await dbContext.Productos.FindAsync(new object[] { id }, cancellationToken);
            if (producto == null) return NotFound();

            // Validar categoría existente antes de persistir
            var categoriaExists = await dbContext.Categorias
                .AnyAsync(c => c.Id == productoDTO.CategoriaId, cancellationToken);
            if (!categoriaExists) return BadRequest("La categoría indicada no existe.");

            producto.Nombre = productoDTO.Nombre;
            producto.Descripcion = productoDTO.Descripcion;
            producto.Precio = productoDTO.Precio;
            producto.Stock = productoDTO.Stock;
            producto.CategoriaId = productoDTO.CategoriaId;
    
            await dbContext.SaveChangesAsync(cancellationToken);
            return Ok(producto);
        }

        // DELETE api/productos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id, CancellationToken cancellationToken = default)
        {
            var producto = await dbContext.Productos.FindAsync(new object[] { id }, cancellationToken);
            if (producto == null) return NotFound();

            dbContext.Productos.Remove(producto);
            await dbContext.SaveChangesAsync(cancellationToken);
            return Ok(producto);
        }
    }
}

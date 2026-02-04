using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using PortalAdminEmpleados.Data;
using PortalAdminEmpleados.Modelos;
using PortalAdminEmpleados.Modelos.Entidades;

namespace PortalAdminEmpleados.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {
        private readonly Conexion dbContext;

        public EmpleadosController(Conexion dbContext)
        {
            this.dbContext = dbContext;
        }


        [HttpGet]
        public IActionResult GetAllEmpleado()
        {
            return Ok(dbContext.Empleados.ToList());
        }


        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetEmpleadoById(Guid id)
        {
            var empleado = dbContext.Empleados.Find(id);

            if (empleado == null)
            {
                return NotFound();
            }

            return Ok(empleado);
        }


        [HttpPost]
        public IActionResult AddEmpleado(AddEmpleadoDto addEmpleadoDto)
        {
            var empleado = new Empleados()
            {

                Email = addEmpleadoDto.Email,
                Nombre = addEmpleadoDto.Nombre,
                Salario = addEmpleadoDto.Salario,
                Telefono = addEmpleadoDto.Telefono
            };

            dbContext.Empleados.Add(empleado);
            dbContext.SaveChanges();

            return Ok(empleado);

        }


        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateEmpleado(Guid id, UpdateEmpleadoDto updateEmpleadoDto)
        {
            var empleado = dbContext.Empleados.Find(id);

            if (empleado == null)
            {
                return NotFound();
            }

            empleado.Nombre = updateEmpleadoDto.Nombre;
            empleado.Email = updateEmpleadoDto.Email;
            empleado.Telefono = updateEmpleadoDto.Telefono;
            empleado.Salario = updateEmpleadoDto.Salario;
            dbContext.SaveChanges();

            return Ok(empleado);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteEmpleado(Guid id)
        {
            var empleado = dbContext.Empleados.Find(id);
            if (empleado == null)
            {
                return NotFound();
            }
            dbContext.Empleados.Remove(empleado);
            dbContext.SaveChanges();

            return Ok(empleado);
        } 

    }
}

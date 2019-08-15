using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ConectarDatos;
using System.Data.Entity;

namespace ApiPedidos.Controllers
{
    public class RolController : ApiController
    {
        private PedidosBDEntities dbContext = new PedidosBDEntities();
        [HttpGet]
        public IEnumerable<Rol> Get()
        {
            using (PedidosBDEntities pedidoentities = new PedidosBDEntities())
            {
                return pedidoentities.Rols.ToList().AsQueryable();
            }

        }
        [HttpGet]

        public Rol Get(int id)
        {
            using (PedidosBDEntities pedidosentities = new PedidosBDEntities())
            {
                return pedidosentities.Rols.FirstOrDefault(e => e.idRol == id);
            }
        }

        [HttpPost]

        public IHttpActionResult AgregarRol([FromBody]Rol rol)
        {
            if (ModelState.IsValid)
            {
                dbContext.Rols.Add(rol);
                dbContext.SaveChanges();
                return Ok(rol);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]

        public IHttpActionResult ActualizarRol(int id, [FromBody]Rol rol)
        {
            if (ModelState.IsValid)
            {
                var RolExiste = dbContext.Rols.Count(c => c.idRol == id) > 0;


                if (RolExiste)
                {
                    dbContext.Entry(rol).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    return Ok(rol);
                }
                else
                {
                    return NotFound();
                }

            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete]
        public IHttpActionResult EliminarRol(int id)
        {
            var rol = dbContext.Rols.Find(id);

            if (rol != null)
            {
                dbContext.Rols.Remove(rol);
                dbContext.SaveChanges();
                return Ok(rol);
            }
            else
            {
                return NotFound();
            }
        }
    }
}


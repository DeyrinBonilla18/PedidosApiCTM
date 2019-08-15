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
    public class UsuariosController : ApiController
    {
        private PedidosBDEntities dbContext = new PedidosBDEntities();
        [HttpGet]
        public IEnumerable<Usuario> Get()
        {
            using (PedidosBDEntities pedidoentities = new PedidosBDEntities())
            {
                return pedidoentities.Usuarios.ToList().AsQueryable();
            }

        }
        [HttpGet]

        public Usuario Get(int id)
        {
            using (PedidosBDEntities pedidosentities = new PedidosBDEntities())
            {
                return pedidosentities.Usuarios.FirstOrDefault(e => e.idUsuario == id);
            }
        }

        [HttpPost]

        public IHttpActionResult AgregarUsuario([FromBody]Usuario usu)
        {
            if (ModelState.IsValid)
            {
                dbContext.Usuarios.Add(usu);
                dbContext.SaveChanges();
                return Ok(usu);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]

        public IHttpActionResult ActualizarUsuario(int id, [FromBody]Usuario usu)
        {
            if (ModelState.IsValid)
            {
                var UsuarioExiste = dbContext.Usuarios.Count(c => c.idUsuario == id) > 0;


                if (UsuarioExiste)
                {
                    dbContext.Entry(usu).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    return Ok(usu);
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
        public IHttpActionResult EliminarUsuario(int id)
        {
            var usu = dbContext.Usuarios.Find(id);

            if (usu != null)
            {
                dbContext.Usuarios.Remove(usu);
                dbContext.SaveChanges();
                return Ok(usu);
            }
            else
            {
                return NotFound();
            }
        }
    }
}

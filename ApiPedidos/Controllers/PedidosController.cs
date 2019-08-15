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
    public class PedidosController : ApiController
    {
        private PedidosBDEntities dbContext = new PedidosBDEntities();
        [HttpGet]
        public IEnumerable<Pedido> Get ()
        {
            using (PedidosBDEntities pedidoentities = new PedidosBDEntities())
            {
                return pedidoentities.Pedidoes.ToList().AsQueryable();
            }

        }
        [HttpGet]

        public Pedido Get(int id)
        {
            using (PedidosBDEntities pedidosentities = new PedidosBDEntities())
            {
                return pedidosentities.Pedidoes.FirstOrDefault(e => e.idPedido == id);
            }
        }

        [HttpPost]

        public IHttpActionResult AgregarPedido([FromBody]Pedido ped)
        {
            if (ModelState.IsValid)
            {
                dbContext.Pedidoes.Add(ped);
                dbContext.SaveChanges();
                return Ok(ped);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]

        public IHttpActionResult ActualizarPedido(int id, [FromBody]Pedido ped)
        {
            if (ModelState.IsValid)
            {
                var PedidoExiste = dbContext.Pedidoes.Count(c => c.idPedido == id) > 0;


                if (PedidoExiste)
                {
                    dbContext.Entry(ped).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    return Ok(ped);
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
        public IHttpActionResult EliminarPedido(int id)
        {
            var ped = dbContext.Pedidoes.Find(id);

            if (ped != null)
            {
                dbContext.Pedidoes.Remove(ped);
                dbContext.SaveChanges();
                return Ok(ped);
            }
            else
            {
                return NotFound();
            }
        }
    }
}


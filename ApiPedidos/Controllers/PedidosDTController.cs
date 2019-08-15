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
    public class PedidosDTController : ApiController
    {
        private PedidosBDEntities dbContext = new PedidosBDEntities();

        [HttpGet]
        public IEnumerable<PedidoDetalle> Get()
        {
            using (PedidosBDEntities pedidoDTentities = new PedidosBDEntities())
            {
                return pedidoDTentities.PedidoDetalles.ToList().AsQueryable();
            }

        }
        [HttpGet]

        public PedidoDetalle Get(int id)
        {
            using (PedidosBDEntities pedidosentities = new PedidosBDEntities())
            {
                return pedidosentities.PedidoDetalles.FirstOrDefault(e => e.idPedidoDT == id);
            }
        }

        [HttpPost]

        public IHttpActionResult AgregarProductoDT([FromBody]PedidoDetalle peDT)
        {
            if (ModelState.IsValid)
            {
                dbContext.PedidoDetalles.Add(peDT);
                dbContext.SaveChanges();
                return Ok(peDT);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]

        public IHttpActionResult ActualizarPedidoDT(int id, [FromBody]PedidoDetalle peDT)
        {
            if (ModelState.IsValid)
            {
                var PedidoDTExiste = dbContext.PedidoDetalles.Count(c => c.idPedidoDT == id) > 0;


                if (PedidoDTExiste)
                {
                    dbContext.Entry(peDT).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    return Ok(peDT);
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
        public IHttpActionResult EliminarPedidoDT(int id)
        {
            var PDT = dbContext.PedidoDetalles.Find(id);

            if (PDT != null)
            {
                dbContext.PedidoDetalles.Remove(PDT);
                dbContext.SaveChanges();
                return Ok(PDT);
            }
            else
            {
                return NotFound();
            }
        }
    }
}


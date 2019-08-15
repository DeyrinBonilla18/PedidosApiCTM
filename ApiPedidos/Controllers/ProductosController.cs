using ConectarDatos;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiPedidos.Controllers
{
    public class ProductosController : ApiController
    {
        private PedidosBDEntities dbContext = new PedidosBDEntities();
      
        [HttpGet]
        public IEnumerable<Producto> Get ()
        {
            using (PedidosBDEntities productoentities = new PedidosBDEntities())
            {
                return productoentities.Productoes.ToList().AsQueryable();
            }

        }

        [HttpGet]

        public Producto Get(int id)
        {
            using (PedidosBDEntities pedidosentities = new PedidosBDEntities())
            {
                return pedidosentities.Productoes.FirstOrDefault(e => e.idProducto == id);
            }
        }

        [HttpPost]

        public IHttpActionResult AgregarProducto([FromBody]Producto pro)
        {
            if (ModelState.IsValid)
            {
                dbContext.Productoes.Add(pro);
                dbContext.SaveChanges();
                return Ok(pro);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut]

        public IHttpActionResult ActualizarProducto(int id, [FromBody]Producto pro)
        {
            if (ModelState.IsValid)
            {
                var ProductoExiste = dbContext.Productoes.Count(c => c.idProducto == id) > 0;


                if (ProductoExiste)
                {
                    dbContext.Entry(pro).State = EntityState.Modified;
                    dbContext.SaveChanges();
                    return Ok(pro);
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
        public IHttpActionResult EliminarProducto(int id)
        {
            var reg = dbContext.Productoes.Find(id);

            if (reg != null)
            {
                dbContext.Productoes.Remove(reg);
                dbContext.SaveChanges();
                return Ok(reg);
            }
            else
            {
                return NotFound();
            }
        }
    }
}


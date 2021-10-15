using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using PaymentAp.Models;

namespace PaymentAp.Controllers
{
    public class clientsController : ApiController
    {
        private BankingDbEntities db = new BankingDbEntities();

        // GET: api/clients
        public IQueryable<client> Getclients()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.clients;
        }
        // GET: api/clients/5
        [ResponseType(typeof(client))]
        public async Task<IHttpActionResult> Getclient(int id)
        {
            client client = await db.clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            return Ok(client);
        }

        // PUT: api/clients/5
        [ResponseType(typeof(void))]
        // POST: api/clients
        [ResponseType(typeof(client))]
        public async Task<IHttpActionResult> Postclient(client client)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.clients.Add(client);
            await db.SaveChangesAsync();
            return CreatedAtRoute("DefaultApi", new { id = client.id }, client);
        }

    

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        [Route("clientExists")]
        [HttpGet]
        public bool clientExists(string otp)
        {
            return db.clients.Count(e => e.tokenkey == otp && e.expdate <DateTime.Now) > 0;
        }
        [Route("clientExistsByAccountNum")]
        [HttpGet]
        public bool clientExistsByAccountNum(string AccNum)
        {
            return db.clients.Count(e => e.id == AccNum) > 0;
        }
    }
}
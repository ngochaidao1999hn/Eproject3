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
    public class BanksController : ApiController
    {
        private BankingDbEntities db = new BankingDbEntities();

        // GET: api/Banks
        public IQueryable<Bank> GetBanks()
        {
            return db.Banks;
        }

        // GET: api/Banks/5
        [ResponseType(typeof(Bank))]
        public async Task<IHttpActionResult> GetBank(int id)
        {
            Bank bank = await db.Banks.FindAsync(id);
            if (bank == null)
            {
                return NotFound();
            }

            return Ok(bank);
        }

        // PUT: api/Banks/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutBank(int id, Bank bank)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != bank.id)
            {
                return BadRequest();
            }

            db.Entry(bank).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BankExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Banks
        [ResponseType(typeof(Bank))]
        public async Task<IHttpActionResult> PostBank(Bank bank)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Banks.Add(bank);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = bank.id }, bank);
        }

        // DELETE: api/Banks/5
        [ResponseType(typeof(Bank))]
        public async Task<IHttpActionResult> DeleteBank(int id)
        {
            Bank bank = await db.Banks.FindAsync(id);
            if (bank == null)
            {
                return NotFound();
            }

            db.Banks.Remove(bank);
            await db.SaveChangesAsync();

            return Ok(bank);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BankExists(int id)
        {
            return db.Banks.Count(e => e.id == id) > 0;
        }
    }
}
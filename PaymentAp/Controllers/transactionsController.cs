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
    public class transactionsController : ApiController
    {
        private BankingDbEntities db = new BankingDbEntities();
        private static Random random = new Random();
        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        // GET: api/transactions
        public IQueryable<transaction> Gettransactions()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.transactions;
        }
        // GET: api/transactions/5
        [ResponseType(typeof(transaction))]
        public async Task<IHttpActionResult> Gettransaction(int id)
        {
            transaction transaction = await db.transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(transaction);
        }

        // PUT: api/transactions/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> Puttransaction(int id, transaction transaction)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transaction.id)
            {
                return BadRequest();
            }

            db.Entry(transaction).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!transactionExists(id))
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

        // POST: api/transactions
        [ResponseType(typeof(transaction))]
        [HttpGet]
        public int Posttransaction(string aNumSender, double amount)
        {
            var sender = db.clients.Find(aNumSender);
            if (sender != null)
            {
                if (sender.balance < amount)
                {
                    return 2;
                }
                sender.balance -= (float)amount;
                db.clients.Find(8).balance += (float)amount;
                transaction model = new transaction()
                {
                    senderId = aNumSender,
                    receiverId = "0711000261892",
                    amount = (float)amount
                };
                db.transactions.Add(model);
                db.SaveChanges();
                return 1;
            }
            else
            {
                return 0;
            }
        }
        [HttpGet]
        [Route("genToken")]
        public string genToken(string accNum)
        {
            string otp = RandomString(6);
            var isValid = db.clients.Find(accNum);
            if (isValid != null)
            {
                isValid.tokenkey = otp;
                isValid.expdate = DateTime.Now.AddMinutes(15);
                db.SaveChanges();
                return otp;
            }
            else
            {
                return "NO FOUND";
            }
   
        }
        // DELETE: api/transactions/5
        [ResponseType(typeof(transaction))]
        public async Task<IHttpActionResult> Deletetransaction(int id)
        {
            transaction transaction = await db.transactions.FindAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }

            db.transactions.Remove(transaction);
            await db.SaveChangesAsync();

            return Ok(transaction);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool transactionExists(int id)
        {
            return db.transactions.Count(e => e.id == id) > 0;
        }
    }
}
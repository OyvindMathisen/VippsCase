using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VippsCaseAPI.DataAccess;
using VippsCaseAPI.Models;

namespace VippsCaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentInfosController : ControllerBase
    {
        private readonly DBContext _context;

        public PaymentInfosController(DBContext context)
        {
            _context = context;
        }

        // GET: api/PaymentInfos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PaymentInfo>>> GetpaymentInfos()
        {
            return await _context.paymentInfos.ToListAsync();
        }

        // GET: api/PaymentInfos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PaymentInfo>> GetPaymentInfo(int id)
        {
            var paymentInfo = await _context.paymentInfos.FindAsync(id);

            if (paymentInfo == null)
            {
                return NotFound();
            }

            return paymentInfo;
        }

        // PUT: api/PaymentInfos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaymentInfo(int id, PaymentInfo paymentInfo)
        {
            if (id != paymentInfo.PaymentInfoId)
            {
                return BadRequest();
            }

            _context.Entry(paymentInfo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentInfoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/PaymentInfos
        [HttpPost]
        public async Task<ActionResult<PaymentInfo>> PostPaymentInfo(PaymentInfo paymentInfo)
        {
            _context.paymentInfos.Add(paymentInfo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPaymentInfo", new { id = paymentInfo.PaymentInfoId }, paymentInfo);
        }

        // DELETE: api/PaymentInfos/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PaymentInfo>> DeletePaymentInfo(int id)
        {
            var paymentInfo = await _context.paymentInfos.FindAsync(id);
            if (paymentInfo == null)
            {
                return NotFound();
            }

            _context.paymentInfos.Remove(paymentInfo);
            await _context.SaveChangesAsync();

            return paymentInfo;
        }

        private bool PaymentInfoExists(int id)
        {
            return _context.paymentInfos.Any(e => e.PaymentInfoId == id);
        }
    }
}

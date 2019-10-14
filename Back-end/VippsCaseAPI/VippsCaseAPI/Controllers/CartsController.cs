using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using VippsCaseAPI.DataAccess;
using VippsCaseAPI.Models;

namespace VippsCaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly DBContext _context;

        public CartsController(DBContext context)
        {
            _context = context;
        }

        // GET: api/Carts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cart>>> Getcarts()
        {
            //_context.
            return await _context.carts.ToListAsync();
        }

        // GET: api/Carts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetCart(int id)
        {
            var cart = await _context.carts.FindAsync(id);

            if (cart == null)
            {
                return NotFound();
            }

            return cart;
        }

        [HttpPost("newCart")]
        public async Task<ActionResult> PostNewCart([FromBody]JObject data)
        {
            //TODO: Seperation of concern
            //TODO: Exception handling
            Random rand = new Random();

            int itemAmount = rand.Next(5);

            Cart cart = new Cart();

            List<Item> items = await _context.items.ToListAsync();

            for (int i = 0; i < itemAmount; i++)
            {
                Random rdm = new Random();
                int index = rdm.Next(items.Count());
                cart.items.Add(items[index]);
            }

            cart.Status = Statuses.InProgress;

            cart.UserId = data["userId"].ToObject<int>();

            _context.carts.Add(cart);

            await _context.SaveChangesAsync();

            return Ok(cart);
        }

        // PUT: api/Carts/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCart(int id, Cart cart)
        {
            if (id != cart.CartId)
            {
                return BadRequest();
            }

            _context.Entry(cart).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CartExists(id))
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

        // POST: api/Carts
        [HttpPost]
        public async Task<ActionResult<Cart>> PostCart(Cart cart)
        {
            _context.carts.Add(cart);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCart", new { id = cart.CartId }, cart);
        }

        // DELETE: api/Carts/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Cart>> DeleteCart(int id)
        {
            var cart = await _context.carts.FindAsync(id);
            if (cart == null)
            {
                return NotFound();
            }

            _context.carts.Remove(cart);
            await _context.SaveChangesAsync();

            return cart;
        }

        private bool CartExists(int id)
        {
            return _context.carts.Any(e => e.CartId == id);
        }
    }
}

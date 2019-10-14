﻿using System;
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
    public class OrdersController : ControllerBase
    {
        private readonly DBContext _context;

        public OrdersController(DBContext context)
        {
            _context = context;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> Getorders()
        {
            return await _context.orders.ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.OrderId)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        [HttpPost("newCart")]
        public async Task<ActionResult> PostNewCart([FromBody]JObject data)
        {
            //TODO: Seperation of concern
            //TODO: Exception handling
            Order o = new Order();
            o.UserId = data["userId"].ToObject<int>();

            _context.orders.Add(o);
            await _context.SaveChangesAsync();

            //TODO: Use last here? possible problem with multiple requests at once
            Order o2 = await _context.orders.LastAsync();

            Random rand = new Random();

            int itemAmount = rand.Next(5);

            List<Item> items = await _context.items.ToListAsync();
            List<Item> cart = new List<Item>();

            for (int i = 0; i < itemAmount; i++)
            {
                Random rdm = new Random();
                int index = rdm.Next(items.Count());
                cart.Add(items[index]);
                OrderItem tempOrderItem = new OrderItem(o2.OrderId, items[index].ItemId);
                _context.orderItems.Add(tempOrderItem);
            }

            await _context.SaveChangesAsync();

            return Ok(cart);
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder([FromBody]JObject data)
        {
            //Get data from post body
            int userId = data["userId"].ToObject<int>();
            int cartId = data["cartId"].ToObject<int>();

            //Retrieve connected cart
            //Cart cart = await _context.carts.FirstOrDefaultAsync(x => x.CartId == cartId);

            //Set order values
            Order order = new Order();

            //Create orderitem values and calculate total cart price
            /*foreach(Item item in cart.items)
            {
                OrderItem tempOrderItem = new OrderItem(order.OrderId);

            }*/

            //Save order
            /*_context.orders.Add(order);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetOrder", new { id = order.OrderId }, order);*/

            return Ok(order);
        }

        [HttpPut("toggleActive/{id}")]
        public async Task<IActionResult> UpdateActiveStatus(int id)
        {
            Order order = new Order();

            order = await _context.orders.FirstOrDefaultAsync(x => x.OrderId == id);

            order.Active = !order.Active;

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
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

        private bool OrderExists(int id)
        {
            return _context.orders.Any(e => e.OrderId == id);
        }
    }
}

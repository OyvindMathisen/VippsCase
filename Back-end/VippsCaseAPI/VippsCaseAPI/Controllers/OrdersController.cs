using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using VippsCaseAPI.DataAccess;
using VippsCaseAPI.DTOs;
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
            return await _context.orders.Where(x => x.Active == true).ToListAsync();
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

        // GET: api/orders/getOrdersByUserID/4
        [HttpGet("getOrdersByUserID/{id}")]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByUserID(int id)
        {
            List<Order> orderList = await _context.orders.Where(x => x.UserId == id && x.Active == true).ToListAsync();

            if (orderList == null)
            {
                return NotFound();
            }

            List<OrderByUserIdDTO> listToReturn = new List<OrderByUserIdDTO>();

            foreach(Order order in orderList)
            {
                OrderByUserIdDTO temp = new OrderByUserIdDTO();
                temp.Order = order;
                List<OrderItem> orderItemList = await _context.orderItems.Where(x => x.OrderId == order.OrderId).ToListAsync();

                foreach (OrderItem orderItem in orderItemList)
                {
                    Item i = await _context.items.FirstOrDefaultAsync(x => x.ItemId == orderItem.ItemId);
                    temp.Items.Add(i);
                }

                listToReturn.Add(temp);
            }

            return Ok(listToReturn);
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
            // Generate an idempotency token to ensure our requests are only handled once, in case of connection issues, etc.
            o2.IdempotencyToken = Guid.NewGuid().ToString();

            Random rand = new Random();

            int itemAmount = rand.Next(5);

            List<Item> items = await _context.items.Where(x => x.Active == true).ToListAsync();
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

            CartDTO cartToReturn = new CartDTO(o2.OrderId, cart);

            return Ok(cartToReturn);
        }

        [HttpPost("changeOrderStatus")]
        public async Task<ActionResult> ChangeOrderStatus([FromBody]ChangeOrderStatusDTO cos)
        {
            Order o = await _context.orders.FirstOrDefaultAsync(x => x.OrderId == cos.OrderId);

            o.Status = (Statuses)cos.Status;

            _context.Entry(o).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok();
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder([FromBody]JObject data)
        {
            //TODO: Validation of data
            //Get data from post body
            int orderId = data["orderId"].ToObject<int>();

            //Retrieve connected cart

            Order order = await _context.orders.SingleOrDefaultAsync(x => x.OrderId == orderId);

            //Set Cart to an in progress order
            order.Status = Statuses.InProgress;

            return Ok(order);
        }

        [HttpPut("toggleActive/{id}")]
        public async Task<IActionResult> UpdateActiveStatus(int id)
        {
            Order order = new Order();

            order = await _context.orders.FirstOrDefaultAsync(x => x.OrderId == id);

            order.Active = !order.Active;

            List<OrderItem> orderItemList = await _context.orderItems.Where(x => x.OrderId == id).ToListAsync();

            foreach(OrderItem oi in orderItemList)
            {
                oi.Active = !order.Active;
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

        private bool OrderExists(int id)
        {
            return _context.orders.Any(e => e.OrderId == id);
        }
    }
}

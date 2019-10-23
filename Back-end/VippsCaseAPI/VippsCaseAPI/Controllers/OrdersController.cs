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
        public async Task<ActionResult> GetOrder(int id)
        {
            GenericResponseDTO genericResponseDTO = new GenericResponseDTO();

            var order = await _context.orders.FindAsync(id);

            if (order == null)
            {
                genericResponseDTO.Message = "There is no order with the given identifier";
                return NotFound(genericResponseDTO);
            }

            return Ok(order);
        }

        // GET: api/orders/getOrdersByUserID/4
        [HttpGet("getOrdersByUserID/{id}")]
        public async Task<ActionResult> GetOrdersByUserID(int id)
        {
            GenericResponseDTO genericResponseDTO = new GenericResponseDTO();

            List<Order> orderList = await _context.orders.Where(x => x.UserId == id && x.Active == true).ToListAsync();

            if (orderList.Count < 1)
            {
                genericResponseDTO.Message = "The user does not currenty have any registered orders.";
                return NotFound(genericResponseDTO);
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
        public async Task<ActionResult> PostNewCart([FromBody]UserIdDTO u)
        {
            //TODO: Seperation of concern
            //TODO: Exception handling
            Order o = new Order();
            GenericResponseDTO genericResponseDTO = new GenericResponseDTO();

            //Check if user exists
            if (u.UserId != 0)
            {
                try
                {
                    await _context.users.FirstAsync(x => x.UserId == u.UserId);
                }
                catch
                {
                    genericResponseDTO.Message = "No user with that ID found";
                    return NotFound(genericResponseDTO);
                }

                o.UserId = u.UserId;
            }
            else
            {
                o.UserId = 1;
            }

            _context.orders.Add(o);
            await _context.SaveChangesAsync();

            // Generate an idempotency token to ensure our requests are only handled once, in case of connection issues, etc.
            o.IdempotencyToken = Guid.NewGuid().ToString();

            Random rand = new Random();

            int itemAmount = rand.Next(5);

            List<Item> items = await _context.items.Where(x => x.Active == true).ToListAsync();
            List<Item> cart = new List<Item>();

            for (int i = 0; i < itemAmount; i++)
            {
                Random rdm = new Random();
                int index = rdm.Next(items.Count());
                cart.Add(items[index]);
                OrderItem tempOrderItem = new OrderItem(o.OrderId, items[index].ItemId);
                _context.orderItems.Add(tempOrderItem);
            }

            await _context.SaveChangesAsync();

            CartDTO cartToReturn = new CartDTO(o.OrderId, cart);

            return Ok(cartToReturn);
        }

        [HttpPost("changeOrderStatus")]
        public async Task<ActionResult> ChangeOrderStatus([FromBody]ChangeOrderStatusDTO cos)
        {
            GenericResponseDTO genericResponseDTO = new GenericResponseDTO();
            Order o = new Order();

            //Check if order exist
            try
            {
                o = await _context.orders.FirstOrDefaultAsync(x => x.OrderId == cos.OrderId);
            }
            catch
            {
                genericResponseDTO.Message = "No order with that order Id exist";
                return NotFound(genericResponseDTO);
            }

            //Check if status is valid
            if (cos.Status >= 0 && cos.Status <= 4)
            {
                o.Status = (Statuses)cos.Status;
            }
            else
            {
                genericResponseDTO.Message = "The status is not valid. Try one of the following:" +
                                                "\n0. In progress" +
                                                "\n1. Accepted" +
                                                "\n2. Declined" +
                                                "\n3. Cart";

                return BadRequest(genericResponseDTO);
            }

            _context.Entry(o).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            genericResponseDTO.Message = "Order status was successfully changed";
            return Ok(genericResponseDTO);
        }

        [HttpPost("toggleActive/{id}")]
        public async Task<ActionResult> UpdateActiveStatus(int id)
        {
            Order order = new Order();
            GenericResponseDTO genericResponseDTO = new GenericResponseDTO();

            //Check if order exist
            try
            {
                order = await _context.orders.FirstOrDefaultAsync(x => x.OrderId == id);
            }
            catch
            {
                genericResponseDTO.Message = "No order with the gice id exist";
                return NotFound(genericResponseDTO);
            }

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
                throw;
            }

            genericResponseDTO.Message = "Active id was successfully changed";
            return Ok(genericResponseDTO);
        }

        private bool OrderExists(int id)
        {
            return _context.orders.Any(e => e.OrderId == id);
        }
    }
}

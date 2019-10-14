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
    public class OrderItemsController : ControllerBase
    {
        private readonly DBContext _context;

        public OrderItemsController(DBContext context)
        {
            _context = context;
        }

        // GET: api/OrderItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderItem>>> GetorderItems()
        {
            return await _context.orderItems.Where(x => x.Active == true).ToListAsync();
        }

        // GET: api/OrderItems/5
        [HttpGet("{orderId}")]
        public async Task<ActionResult<List<OrderItem>>> GetOrderItemByOrderId(int orderId)
        {
            List<OrderItem> orderItems = await _context.orderItems.Where(x => x.OrderId == orderId).ToListAsync();

            if (orderItems == null)
            {
                return NotFound();
            }

            return orderItems;
        }
    }
}

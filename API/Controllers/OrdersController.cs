using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly DataContext _context;
        public OrdersController(DataContext context)
        {
            _context = context;
           
        }

        // api/orders/
        // endPoint to get all orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders.ToListAsync();
        }

        // api/orders/1
        // endPoint to get a specific order
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var myOrder = await _context.Orders.FindAsync(id);

            if (myOrder == null)
            {
                return NotFound();
            }
            return myOrder;
        }

        // endPoint to add new order
        [HttpPost]
        public async Task<ActionResult<Order>> AddOrder(Order order)
        {
             _context.Orders.AddAsync(order);
             //Save changes in the DB
             await _context.SaveChangesAsync();
            // return the order inserted in DB
             return CreatedAtAction("GetOrder", new {id=order.Id}, order);        
        }

        // EndPoint to update existing order
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, Order order)
        {
            //Check if the order exists 
            if (id != order.Id)
            {
                return BadRequest();
            }
            // if the order id exists, we update the context 
            _context.Entry(order).State = EntityState.Modified;

            try
            {
                // Save the changes data to DB
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if(!OrderExists(id))
                    return NotFound();
                else    
                    throw;
            } 
            return NoContent(); 

        }

        // EndPoint to delete existing order
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            // Check if the order exists
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
              return NotFound(); 
            }
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return NoContent();

        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(x => x.Id == id );
        }
    }
}
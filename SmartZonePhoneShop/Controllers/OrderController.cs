using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartZonePhoneShop.Data;
using SmartZonePhoneShop.DTO.OrderDTOs;

using SmartZonePhoneShop.Interface;
using SmartZonePhoneShop.Model;
using SmartZonePhoneShop.Repository;

namespace SmartZonePhoneShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public OrderController(IOrderRepository orderRepository, IMapper mapper, ApplicationDbContext context)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _context = context;
        }

        // GET: api/Order
        
        [HttpGet]
        [Authorize]
        public IActionResult GetOrders()
        {
            var orders = _orderRepository.GetOrders();
            return Ok(orders);
        }

        // GET: api/Order/5
        [Authorize(Policy = "Administrator")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetOrderById(int id)
        {
            var order = _orderRepository.GetOrderByID(id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // POST: api/Order
        [Authorize(Policy = "Kupac")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO orderDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = new Order
            {
                UserId = orderDTO.UserId,
                Street = orderDTO.Street,
                City = orderDTO.City,
                Quantity = orderDTO.Quantity,
                PaymentSum = orderDTO.PaymentSum,
                PaymentMethod = orderDTO.PaymentMethod,
                OrderId = orderDTO.OrderId,
                OrderStatusId = orderDTO.OrderStatusId,             
                Date = DateTime.Now,
                // Set other properties of the Order object from the OrderDTO as needed
            };

            await _orderRepository.AddAsync(order);

            return CreatedAtAction(nameof(GetOrderById), new { id = order.OrderId }, order);
        }



        // PUT: api/Order/5
        [Authorize(Policy = "Administrator")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderDTO orderDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userExists = _context.Users.Any(r => r.UserId == orderDTO.UserId);

            if (!userExists)
            {
                ModelState.AddModelError("", "User does not exists");
                return StatusCode(422, ModelState);
            }
            var orderStatusExists = _context.Products.Any(r => r.ProductId == orderDTO.OrderStatusId);

            if (!orderStatusExists)
            {
                ModelState.AddModelError("", "Order status does not exists");
                return StatusCode(422, ModelState);
            }

            var order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
            {
                return NotFound("Order does not exist");
            }

            // Update the properties of the order object from the orderDTO as needed
            order.UserId = orderDTO.UserId;
            order.OrderId = orderDTO.OrderId;
            order.OrderStatusId = orderDTO.OrderStatusId;
            order.Street = orderDTO.Street;
            order.City = orderDTO.City;
            order.Date = orderDTO.Date;
            order.Quantity = orderDTO.Quantity;
            order.PaymentSum = orderDTO.PaymentSum;
            order.PaymentMethod = orderDTO.PaymentMethod;

            await _orderRepository.UpdateAsync(order);
            return NoContent();
        }




        // DELETE: api/Order/5
        [Authorize(Policy = "Administrator")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return NotFound("Order does not exist");
            }

            await _orderRepository.DeleteAsync(order);

            return NoContent();
        }
    }
}

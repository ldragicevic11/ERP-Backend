using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartZonePhoneShop.Data;
using SmartZonePhoneShop.DTO.CartItemDTOs;
using SmartZonePhoneShop.DTO.OrderItemDTOs;
using SmartZonePhoneShop.DTO.ReviewDTOs;
using SmartZonePhoneShop.Interface;
using SmartZonePhoneShop.Model;

namespace SmartZonePhoneShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public OrderItemController(IOrderItemRepository orderItemRepository, IMapper mapper, ApplicationDbContext context)
        {
            _orderItemRepository = orderItemRepository;
            _mapper = mapper;
            _context = context;
        }
        [Authorize(Policy = "Administrator")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetOrderItems()
        {
            var orderItems = _mapper.Map<List<OrderItemDTO>>(_orderItemRepository.GetOrderItems());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(orderItems);
        }




        // GET: api/Review
        // GET api/user/{userId}/reviews
        [Authorize(Policy = "Administrator")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetOrderItem(int id)
        {
            var orderItem = await _orderItemRepository.GetByIdAsync(id);
            if (orderItem == null)
            {
                return NotFound("Order item does not exist"); ;
            }
            var orderItemDTO = _mapper.Map<OrderItemDTO>(_orderItemRepository.GetOrderItemByID(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(orderItemDTO);
        }

        // POST api/user/{userId}/reviews
        [Authorize(Policy = "Kupac")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddOrderItem(CreateOrderItemDTO orderItemDTO)
        {
            var orderExists = _context.Orders.Any(r => r.OrderId == orderItemDTO.OrderId);

            if (!orderExists)
            {
                ModelState.AddModelError("", "Order does not exists");
                return StatusCode(422, ModelState);
            }

            var productExists = _context.Products.Any(r => r.ProductId == orderItemDTO.ProductId);

            if (!productExists)
            {
                ModelState.AddModelError("", "Product does not exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orderItem = _mapper.Map<OrderItem>(orderItemDTO);

            await _orderItemRepository.AddAsync(orderItem);
            return CreatedAtAction(nameof(GetOrderItem), new { id = orderItem.OrderItemId }, orderItem);
        }

        // PUT api/user/{userId}/reviews/{reviewId}
        [Authorize(Policy = "Kupac")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Put(int id, UpdateOrderItemDTO orderItemDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orderItem = await _orderItemRepository.GetByIdAsync(id);
            if (orderItem == null)
            {
                return NotFound("Order item does not exist");
            }

            var orderExists = _context.Orders.Any(r => r.OrderId == orderItemDTO.OrderId);

            if (!orderExists)
            {
                ModelState.AddModelError("", "Order does not exists");
                return StatusCode(422, ModelState);
            }

            var productExists = _context.Products.Any(r => r.ProductId == orderItemDTO.ProductId);

            if (!productExists)
            {
                ModelState.AddModelError("", "Product does not exists");
                return StatusCode(422, ModelState);
            }

            orderItem.OrderItemId = orderItemDTO.OrderItemId;
            orderItem.ProductPrice = orderItemDTO.ProductPrice;
            orderItem.Quantity = orderItemDTO.Quantity;
            orderItem.OrderId = orderItemDTO.OrderId;
            orderItem.ProductId = orderItemDTO.ProductId;


            await _orderItemRepository.UpdateAsync(orderItem);

            return NoContent();
        }

        // DELETE api/user/{userId}/reviews/{reviewId}
        [Authorize(Policy = "Administrator")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(int id)
        {
            var review = await _orderItemRepository.GetByIdAsync(id);
            if (review == null)
            {
                return NotFound("Order item does not exist");
            }

            await _orderItemRepository.DeleteAsync(review);

            return NoContent();
        }

    }
}

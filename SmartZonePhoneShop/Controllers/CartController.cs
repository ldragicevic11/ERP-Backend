using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartZonePhoneShop.Data;
using SmartZonePhoneShop.DTO.CartDTOs;
using SmartZonePhoneShop.DTO.OrderDTOs;
using SmartZonePhoneShop.Interface;
using SmartZonePhoneShop.Model;

namespace SmartZonePhoneShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public CartController(ICartRepository cartRepository, ICartItemRepository cartItemRepository, IMapper mapper, ApplicationDbContext context)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _mapper = mapper;
            _context = context;
        }

        // GET: api/Order
        [HttpGet]
        public IActionResult GetCarts()
        {
            var carts = _cartRepository.GetCarts();
            return Ok(carts);
        }

        // GET: api/Order/5
        [Authorize(Policy = "Kupac")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetCartById(int id)
        {
            var order = _cartRepository.GetCartByID(id);

            if (order == null)
            {
                return NotFound("Cart does not exist");
            }

            return Ok(order);
        }

        [Authorize(Policy = "Kupac")]
        [HttpGet("users/{userId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetCartByUserId(int userId)
        {
            var cart = _cartRepository.GetCartByUserID(userId);

            if (cart == null)
            {
                return Ok(new Cart
                {
                    CartItems = new List<CartItem>()
                });
            }

            return Ok(cart);
        }

        [Authorize(Policy = "Administrator")]
        [HttpGet("GetCartsByCustomer/{userId}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCartsByCustomerAsync(int userId)
        {
            var carts = await _cartRepository.GetCartsByCustomerAsync(userId);

            if (carts == null || !carts.Any())
            {
                return NotFound("Carts do not exist for the given user");
            }

            return Ok(carts);
        }

        // POST: api/Order
        [Authorize(Policy = "Kupac")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateCart([FromBody] CreateCartDTO cartDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cart = new Cart
            {
                UserId = cartDTO.UserId,
                CartId = cartDTO.CartId
               
                
                // Set other properties of the Order object from the OrderDTO as needed
            };

            await _cartRepository.AddAsync(cart);

            return CreatedAtAction(nameof(GetCartById), new { id = cart.CartId }, cart);
        }



        // PUT: api/Order/5

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateCart(int id, [FromBody] UpdateCartDTO cartDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userExists = _context.Users.Any(r => r.UserId == cartDTO.UserId);

            if (!userExists)
            {
                ModelState.AddModelError("", "User does not exists");
                return StatusCode(422, ModelState);
            }

            var cart = await _cartRepository.GetByIdAsync(id);

            if (cart == null)
            {
                return NotFound("Cart does not exist");
            }

            // Update the properties of the order object from the orderDTO as needed
            cart.UserId = cartDTO.UserId;
            cart.CartId = cartDTO.CartId;
           

            await _cartRepository.UpdateAsync(cart);
            return NoContent();
        }




        // DELETE: api/Order/5
        [Authorize(Policy = "Kupac")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(int id)
        {
            var cart = _cartRepository.GetCartWithCartItems(id);
            if (cart == null)
            {
                return NotFound("Cart does not exist");
            }

            foreach(var item  in cart.CartItems)
            {
                item.Product.OnStock++;
            }

            await _context.SaveChangesAsync();

            _context.Set<Cart>().Remove(cart);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

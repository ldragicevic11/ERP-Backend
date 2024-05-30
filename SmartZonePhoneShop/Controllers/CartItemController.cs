using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartZonePhoneShop.Data;
using SmartZonePhoneShop.DTO.CartItemDTOs;
using SmartZonePhoneShop.DTO.ReviewDTOs;
using SmartZonePhoneShop.Interface;
using SmartZonePhoneShop.Model;
using SmartZonePhoneShop.Repository;

namespace SmartZonePhoneShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemController : ControllerBase
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public CartItemController(ICartItemRepository cartItemRepository, ICartRepository cartRepository, IMapper mapper, ApplicationDbContext context)
        {
            _cartItemRepository = cartItemRepository;
            _cartRepository = cartRepository;
            _mapper = mapper;
            _context = context;
        }

        [Authorize(Policy = "Administrator")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCartItems()
        {
            var cartItems = _mapper.Map<List<CartItemDTO>>(_cartItemRepository.GetCartItems());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(cartItems);
        }




        // GET: api/Review
        // GET api/user/{userId}/reviews
        [Authorize(Policy = "Kupac")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCartItem(int id)
        {
            var cartItem = await _cartItemRepository.GetByIdAsync(id);
            if (cartItem == null)
            {
                return NotFound("Cart item does not exist"); ;
            }
            var cartItemDTO = _mapper.Map<CreateCartItemDTO>(_cartItemRepository.GetCartItemByID(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(cartItemDTO);
        }

        // POST api/user/{userId}/reviews
        [Authorize(Policy = "Kupac")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddCartItem(CreateCartItemDTO cartItemDTO)
        {
            var cart = _context.Carts.FirstOrDefault(r => r.UserId == cartItemDTO.UserId);
            
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = cartItemDTO.UserId
                };

                await _cartRepository.AddAsync(cart);
            }

            var product = _context.Products.FirstOrDefault(r => r.ProductId == cartItemDTO.ProductId);

            if (product == null)
            {
                ModelState.AddModelError("", "Product does not exists");
                return StatusCode(422, ModelState);
            }

            if (product.OnStock == 0)
            {
                ModelState.AddModelError("", "Product is not on the stock");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cartItem = _mapper.Map<CartItem>(cartItemDTO);
            cartItem.SumPrice = product.Price;
            cartItem.CartId = cart.CartId;
            await _cartItemRepository.AddAsync(cartItem);

            product.OnStock -= 1;
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCartItem), new { id = cartItem.CartItemId }, _mapper.Map< CartItemDTO >(cartItem));
        }

        // PUT api/user/{userId}/reviews/{reviewId}
        [Authorize(Policy = "Kupac")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Put(int id, UpdateCartItemDTO cartItemDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cartItem = await _cartItemRepository.GetByIdAsync(id);
            if (cartItem == null)
            {
                return NotFound("Cart item does not exist");
            }

            var cartExists = _context.Carts.Any(r => r.CartId == cartItemDTO.CartId);

            if (!cartExists)
            {
                ModelState.AddModelError("", "Cart does not exists");
                return StatusCode(422, ModelState);
            }

            var productExists = _context.Products.Any(r => r.ProductId == cartItemDTO.ProductId);

            if (!productExists)
            {
                ModelState.AddModelError("", "Product does not exists");
                return StatusCode(422, ModelState);
            }

            cartItem.CartItemId = cartItemDTO.CartItemId;
            cartItem.Quantity = cartItemDTO.Quantity;
            cartItem.SumPrice = cartItemDTO.SumPrice;
            cartItem.CartId = cartItemDTO.CartId;
            cartItem.ProductId = cartItemDTO.ProductId;


            await _cartItemRepository.UpdateAsync(cartItem);

            return NoContent();
        }

        // DELETE api/user/{userId}/reviews/{reviewId}
        [Authorize(Policy = "Kupac")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(int id)
        {
            var cartItem = await _cartItemRepository.GetByIdAsync(id);
            if (cartItem == null)
            {
                return NotFound("Cart item does not exist");
            }

            await _cartItemRepository.DeleteAsync(cartItem);

            return NoContent();
        }

    }
}

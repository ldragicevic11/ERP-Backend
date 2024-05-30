using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartZonePhoneShop.DTO.ProductDTOs;
using SmartZonePhoneShop.Interface;
using SmartZonePhoneShop.Model;

namespace SmartZonePhoneShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        // GET api/product
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ProductDTO>>> Get([FromQuery] string? sort, [FromQuery] string? search)
        {
            var products = await _productRepository.GetAllAsync();

            if (!string.IsNullOrEmpty(search))
            {
                products = products.Where(p => p.Name.ToLower().Contains(search.ToLower()));
            }

            // Sortiranje po ceni opadajuće
            if (sort == "price_desc")
            {
                products = products.OrderByDescending(p => p.Price);
            }

            
            var productDTOs = _mapper.Map<IEnumerable<ProductDTO>>(products);
            return Ok(productDTOs);
        }

        // GET api/product/5
        //[Authorize(Policy = "Administrator")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductDTO>> GetById(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound("User does not exist");
            }
            var productDTO = _mapper.Map<ProductDTO>(product);
            return Ok(productDTO);
        }

        // POST api/product
        [Authorize(Policy = "Administrator")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post(UpdateProductDTO productDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = _mapper.Map<Product>(productDTO);

            await _productRepository.AddAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = product.ProductId }, product);
        }

        // PUT api/product/5
        [Authorize(Policy = "Administrator")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Put(int id, UpdateProductDTO productDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            product.Name = productDTO.Name;
            product.Model = productDTO.Model;
            product.Price = productDTO.Price;
            product.Guarantee = productDTO.Guarantee;
            product.OnStock = productDTO.OnStock;
            

            await _productRepository.UpdateAsync(product);

            return NoContent();
        }

        // DELETE api/product/5
        [Authorize(Policy = "Administrator")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product == null)
            {
                return NotFound("User does not exist");
            }

            await _productRepository.DeleteAsync(product);

            return NoContent();
        }
    }
}


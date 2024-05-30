using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartZonePhoneShop.DTO.OrderStatusDTOs;
using SmartZonePhoneShop.Interface;
using SmartZonePhoneShop.Model;

namespace SmartZonePhoneShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderStatusController : ControllerBase
    {
        private readonly IOrderStatusRepository _orderStatusRepository;
        private readonly IMapper _mapper;

        public OrderStatusController(IOrderStatusRepository orderStatusRepository, IMapper mapper)
        {
            _orderStatusRepository = orderStatusRepository;
            _mapper = mapper;
        }

        // GET api/orderStatus
        [Authorize(Policy = "Administrator")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<OrderStatusDTO>>> Get()
        {
            var orderStatus = await _orderStatusRepository.GetAllAsync();
            var orderStatusDTOs = _mapper.Map<IEnumerable<OrderStatusDTO>>(orderStatus);
            return Ok(orderStatusDTOs);
        }

        // GET api/orderStatus/5
        [Authorize(Policy = "Kupac")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderStatusDTO>> GetById(int id)
        {
            var orderStatus = await _orderStatusRepository.GetByIdAsync(id);
            if (orderStatus == null)
            {
                return NotFound();
            }
            var orderStatusDTO = _mapper.Map<OrderStatus>(orderStatus);
            return Ok(orderStatusDTO);
        }

        // POST api/orderStatus
        [Authorize(Policy = "Administrator")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post(OrderStatusDTO orderStatusDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orderStatus = _mapper.Map<OrderStatus>(orderStatusDTO);

            await _orderStatusRepository.AddAsync(orderStatus);
            return CreatedAtAction(nameof(GetById), new { id = orderStatus.OrderStatusId }, orderStatus);
        }

        // PUT api/orderStatus/5
        [Authorize(Policy = "Administrator")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Put(int id, OrderStatusDTO orderStatusDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orderStatus = await _orderStatusRepository.GetByIdAsync(id);
            if (orderStatus == null)
            {
                return NotFound();
            }

            orderStatus.StatusName = orderStatusDTO.StatusName;

            await _orderStatusRepository.UpdateAsync(orderStatus);

            return NoContent();
        }

        // DELETE api/orderStatus/5
        [Authorize(Policy = "Administrator")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(int id)
        {
            var orderStatus = await _orderStatusRepository.GetByIdAsync(id);
            if (orderStatus == null)
            {
                return NotFound();
            }

            await _orderStatusRepository.DeleteAsync(orderStatus);

            return NoContent();
        }
    }
}

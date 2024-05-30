using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartZonePhoneShop.DTO.UserTypeDTOs;
using SmartZonePhoneShop.Model;
using SmartZonePhoneShop.Interface;
using Microsoft.AspNetCore.Authorization;

namespace SmartZonePhoneShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTypeController : ControllerBase
    {
        private readonly IUserTypeRepository _userTypeRepository;
        private readonly IMapper _mapper;

        public UserTypeController(IUserTypeRepository userTypeRepository, IMapper mapper)
        {
            _userTypeRepository = userTypeRepository;
            _mapper = mapper;
        }

        // GET api/userType
        [Authorize(Policy = "Administrator")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<CreateUserTypeDTO>>> Get()
        {
            var userTypes = await _userTypeRepository.GetAllAsync();
            var userTypeDTOs = _mapper.Map<IEnumerable<CreateUserTypeDTO>>(userTypes);
            return Ok(userTypeDTOs);
        }

        // GET api/userType/5
        [Authorize(Policy = "Administrator")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<CreateUserTypeDTO>> GetById(int id)
        {
            var userType = await _userTypeRepository.GetByIdAsync(id);
            if (userType == null)
            {
                return NotFound("User type does not exist");
            }
            var userTypeDTO = _mapper.Map<CreateUserTypeDTO>(userType);
            return Ok(userTypeDTO);
        }

        // POST api/userType
        [Authorize(Policy = "Administrator")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post(CreateUserTypeDTO userTypeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userType = _mapper.Map<UserType>(userTypeDTO);

            await _userTypeRepository.AddAsync(userType);
            return CreatedAtAction(nameof(GetById), new { id = userType.UserTypeId }, userType);
        }

        // PUT api/userType/5
        [Authorize(Policy = "Administrator")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Put(int id, UpdateUserTypeDTO userTypeDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userType = await _userTypeRepository.GetByIdAsync(id);
            if (userType == null)
            {
                return NotFound();
            }

            userType.Name = userTypeDTO.Name;

            await _userTypeRepository.UpdateAsync(userType);

            return NoContent();
        }

        // DELETE api/userType/5
        [Authorize(Policy = "Administrator")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(int id)
        {
            var userType = await _userTypeRepository.GetByIdAsync(id);
            if (userType == null)
            {
                return NotFound("User type does not exist");
            }

            await _userTypeRepository.DeleteAsync(userType);

            return NoContent();
        }
    }
}

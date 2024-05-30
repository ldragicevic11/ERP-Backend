using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SmartZonePhoneShop.DTO.UserDTOs;
using SmartZonePhoneShop.Model;
using SmartZonePhoneShop.Interface;
using Microsoft.EntityFrameworkCore;
using SmartZonePhoneShop.Data;
using SmartZonePhoneShop.Repository;
using SmartZonePhoneShop.DTO.ReviewDTOs;
using Microsoft.AspNetCore.Authorization;

namespace SmartZonePhoneShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;


        public UserController(IUserRepository userRepository, IMapper mapper, ApplicationDbContext context)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _context = context;
        }

        // GET api/user
        //[Authorize(Policy = "Administrator")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<List<User>> GetAllUsers()
        {
            // Izvršavanje join-a sa tabelom "UserType" kako biste dobili vrednost atributa "Name"
            var users = _context.Users.Include(u => u.UserType).ToList();

            // Mapiranje na novi model koji koristi atribut "Name" umesto "userTypeId"
            var mappedUsers = users.Select(u => new User
            {
                UserId = u.UserId,
                Email = u.Email,
                Name = u.Name,
                Username = u.Username,
                Password = u.Password,
                Contact = u.Contact,
                City = u.City,
                Address = u.Address,
                UserTypeId = u.UserType.UserTypeId,
                UserType = new UserType
                {
                    UserTypeId = u.UserType.UserTypeId,
                    Name = u.UserType.Name // Koristimo atribut "Name" iz tabele "UserType"
                }
            }).ToList(); // Dodajte ToList() na kraju

            return mappedUsers;
        }

        // GET api/user/5
        [Authorize(Policy = "Administrator")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<User>> GetById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound("User does not exist");
            }

            var userType = _context.UserTypes.FirstOrDefault(ut => ut.UserTypeId == user.UserTypeId);

            var newUser = new User
            {
                UserId = user.UserId,
                Email = user.Email,
                Name = user.Name,
                Username = user.Username,
                Password = user.Password,
                Contact = user.Contact,
                City = user.City,
                Address = user.Address,
                UserTypeId = user.UserTypeId,
                UserType = userType != null ? new UserType
                {
                    UserTypeId = userType.UserTypeId,
                    Name = userType.Name
                } : null
            };

            return Ok(newUser);
        }




        // POST api/user
        
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Post(CreateUserDTO userDTO)
        {
            
            var userExist = _userRepository.GetUsers().Where(r => r.Username == userDTO.Username).FirstOrDefault();

            if (userExist != null)
            {
                ModelState.AddModelError("", "Username already exists");
                return StatusCode(422, ModelState);
            }

            var userTypeExists = _context.UserTypes.Any(r => r.UserTypeId == userDTO.UserTypeId);

            if (!userTypeExists)
            {
                ModelState.AddModelError("", "User type does not exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            

            //return BadRequest(ModelState);

            var user = _mapper.Map<User>(userDTO);

            await _userRepository.AddAsync(user);

            if (ModelState.IsValid)
            {


                return Ok("Registered successfully.");
            }

            return CreatedAtAction(nameof(GetById), new { id = user.UserId }, user);

            
        }

        // PUT api/user/5

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Put(int id, UpdateUserDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound("User does not exist");
            }

            user.Email = userDTO.Email;
            user.Name = userDTO.Name;
            user.Username = userDTO.Username;
            user.Password = userDTO.Password;
            user.Contact = userDTO.Contact;
            user.City = userDTO.City;
            user.Address = userDTO.Address;
            user.UserTypeId = userDTO.UserTypeId;

            await _userRepository.UpdateAsync(user);

            return NoContent();
        }

        // DELETE api/user/5
        [Authorize(Policy = "Administrator")]
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Delete(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound("User does not exist");
            }

            await _userRepository.DeleteAsync(user);

            return NoContent();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartZonePhoneShop.Data;
using SmartZonePhoneShop.DTO.ReviewDTOs;
using SmartZonePhoneShop.DTO.UserDTOs;
using SmartZonePhoneShop.Interface;
using SmartZonePhoneShop.Model;
using SmartZonePhoneShop.Repository;

namespace SmartZonePhoneShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public ReviewController(IReviewRepository reviewRepository, IMapper mapper, ApplicationDbContext context)
        {
            _reviewRepository = reviewRepository;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetReviews()
        {
            var reviews = _mapper.Map<List<ReviewDTO>>(_reviewRepository.GetReviews());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviews);
        }

        // GET: api/Review
        // GET api/user/{userId}/reviews
       // [Authorize(Policy = "Administrator")]
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetReview(int id)
        {
            var review = await _reviewRepository.GetByIdAsync(id);
            if (review == null)
            {
                return NotFound("Review does not exist");  ;
            }
             var reviewDTO = _mapper.Map<List<ReviewDTO>>(_reviewRepository.GetReviewsByProductId(id));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(reviewDTO);
        }

        //[HttpGet("user/{userId}")]
        //[ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> GetUserReviews(int userId)
        //{
        //    var userExists = _context.Users.Any(u => u.UserId == userId);

        //    if (!userExists)
        //    {
        //        ModelState.AddModelError("", "User does not exist");
        //        return StatusCode(422, ModelState);
        //    }

        //    var reviews = await _reviewRepository.GetReviewsByUserId(userId).ToListAsync();


        //    if (reviews == null)
        //    {
        //        return NoContent();
        //    }

        //    var reviewDTOs = _mapper.Map<List<ReviewDTO>>(reviews);

        //    return Ok(reviewDTOs);
        //}

        // POST api/user/{userId}/reviews
        [Authorize(Policy = "Kupac")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddReview(CreateReviewDTO reviewDTO)
        {
            var userExists = _context.Users.Any(r => r.UserId == reviewDTO.UserId);

            if (!userExists)
            {
                ModelState.AddModelError("", "User does not exists");
                return StatusCode(422, ModelState);
            }

            var productExists = _context.Products.Any(r => r.ProductId == reviewDTO.ProductId);

            if (!productExists)
            {
                ModelState.AddModelError("", "Product does not exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var review = _mapper.Map<Review>(reviewDTO);

            await _reviewRepository.AddAsync(review);
            return CreatedAtAction(nameof(GetReview), new { id = review.ReviewId }, review);
        }

        // PUT api/user/{userId}/reviews/{reviewId}
        [Authorize(Policy = "Kupac")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> Put(int id, UpdateReviewDTO reviewDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var review = await _reviewRepository.GetByIdAsync(id);
            if (review == null)
            {
                return NotFound("Review does not exist");
            }

            var userExists = _context.Users.Any(r => r.UserId == reviewDTO.UserId);

            if (!userExists)
            {
                ModelState.AddModelError("", "User does not exists");
                return StatusCode(422, ModelState);
            }

            var productExists = _context.Products.Any(r => r.ProductId == reviewDTO.ProductId);

            if (!productExists)
            {
                ModelState.AddModelError("", "Product does not exists");
                return StatusCode(422, ModelState);
            }

            review.ReviewId = reviewDTO.ReviewId;
            review.Comment = reviewDTO.Comment;
            review.UserId = reviewDTO.UserId;
            review.ProductId = reviewDTO.ProductId;
            

            await _reviewRepository.UpdateAsync(review);

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
            var review = await _reviewRepository.GetByIdAsync(id);
            if (review == null)
            {
                return NotFound("Review does not exist");
            }

            await _reviewRepository.DeleteAsync(review);

            return NoContent();
        }

    }
}

using AutoMapper;
using SmartZonePhoneShop.DTO.ReviewDTOs;
using SmartZonePhoneShop.Model;

namespace SmartZonePhoneShop.Profiles
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<Review, CreateReviewDTO>();
            CreateMap<CreateReviewDTO, Review>();

            CreateMap<Review, ReviewDTO>();
            CreateMap<ReviewDTO, Review>();

            CreateMap<Review, UpdateReviewDTO>();
            CreateMap<UpdateReviewDTO, Review>();

        }
    }
}

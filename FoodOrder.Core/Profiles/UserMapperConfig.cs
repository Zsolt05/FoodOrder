using AutoMapper;
using FoodOrder.Core.Models.User;
using FoodOrder.Data.Entities;

namespace FoodOrder.Core.Profiles
{
    public class UserMapperConfig : Profile
    {
        public UserMapperConfig()
        {
            CreateMap<UserRegisterDto, User>();
        }
    }
}

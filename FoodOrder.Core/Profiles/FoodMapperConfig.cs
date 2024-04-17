using AutoMapper;

namespace FoodOrder.Core.Profiles
{
    public class FoodMapperConfig : Profile
    {
        public FoodMapperConfig()
        {
            CreateMap<Data.Entities.Food, Models.Food.FoodDto>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category));
            CreateMap<Models.Food.CreateFoodDto, Data.Entities.Food>();
            CreateMap<Data.Entities.FoodCategory, Models.Food.FoodCategoryDto>();
        }
    }
}

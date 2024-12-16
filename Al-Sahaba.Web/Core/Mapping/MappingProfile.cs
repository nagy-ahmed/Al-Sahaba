using Al_Sahaba.Web.Core.Models;
using Al_Sahaba.Web.Core.ViewModels;
using AutoMapper;

namespace Al_Sahaba.Web.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Domain to ViewModel
            CreateMap<Category, CategoryViewModel>()
                .ForMember(des => des.Name, opt => opt.MapFrom(src => src.Name)); // if the two props with different names

            // FormModel to Domain
            CreateMap<CategoryFormViewModel, Category>().ReverseMap();
        }
    }
}

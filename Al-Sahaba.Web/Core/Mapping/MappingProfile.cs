namespace Al_Sahaba.Web.Core.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            //Category
            // Domain to ViewModel
            CreateMap<Category, CategoryViewModel>()
                .ForMember(des => des.Name, opt => opt.MapFrom(src => src.Name)); // if the two props with different names
            // FormModel to Domain
            CreateMap<CategoryFormViewModel, Category>().ReverseMap();


            //Author
            CreateMap<Author, AuthorViewModel>();
            CreateMap<AuthorFormViewModel, Author>().ReverseMap();

        }
    }
}

using AutoMapper;
using EmployeeDAA.Api.Extensions;
using EmployeeDAA.Api.Models.Category;
using EmployeeDAA.Api.Models.Department;
using EmployeeDAA.Api.Models.Employee;
using EmployeeDAA.Api.Models.Order;
using EmployeeDAA.Api.Models.Product;
using EmployeeDAA.Api.Models.User;
using EmployeeDAA.Core.Domain;
using EmployeeDAA.Core.Domain.Department;
using EmployeeDAA.Core.Domain.Orders;

namespace EmployeeDAA.Api.InfraStructure
{
    public class ApplicationMappingProfile:Profile
    {
        public ApplicationMappingProfile()
        {
            CreateMap<EmployeeModel,Employees>()
            .ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();

            CreateMap<DepartmentModel, Department>()
            .ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<CategoryModel, Categories>()
            .ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<ProductModel, Product>()
           .ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<OrderInfoModel, OrderInfo>()
          .ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<OrderCartModel, OrderCart>()
       .ReverseMap().IgnoreAllPropertiesWithAnInaccessibleSetter();

            //CreateMap<User, UserModel>()
            //.ForMember(dest => dest.Password, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Password) ? null : src.Password));
            CreateMap<UserModel, User>()
               .ReverseMap()
               .IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<RoleModel, Role>()
                .ReverseMap()
                .IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<User, UserModel>()
            .ForMember(dest => dest.Password, opt => opt.MapFrom(src => !string.IsNullOrEmpty(src.Password) ? null : src.Password));
            CreateMap<UserProfileModel, User>()
    .ReverseMap()
    .IgnoreAllPropertiesWithAnInaccessibleSetter();


            CreateMap<ChangePasswordModel, User>()
    .ReverseMap()
    .IgnoreAllPropertiesWithAnInaccessibleSetter();
            CreateMap<CreateUserModel, User>()
.ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => (int)src.RoleId)).IgnoreAllPropertiesWithAnInaccessibleSetter();




        }
    }
}

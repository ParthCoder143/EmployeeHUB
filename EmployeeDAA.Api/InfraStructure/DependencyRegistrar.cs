using EmployeeDAA.Core;
using EmployeeDAA.Data.DataProvider;
using EmployeeDAA.Services;
using EmployeeDAA.Services.Category;
using EmployeeDAA.Services.Employee;
using EmployeeDAA.Services.Order;
using EmployeeDAA.Services.PermissionService;
using EmployeeDAA.Services.Products;
using EmployeeDAA.Services.Settings;
using EmployeeDAA.Services.Users;
using LinqToDB.DataProvider.MySql;

namespace EmployeeDAA.Api.InfraStructure
{
    public class DependencyRegistrar
    {
        public static void Register(IServiceCollection services)
        {
            services.AddScoped<IEmployeeServices, EmployeeServices>();
            services.AddScoped<ICategoryServices, CategoryServices>();
            services.AddScoped<IProductServices, ProductServices>();

            services.AddScoped<IUserService, UserService>();

            services.AddScoped(typeof(IRepository<>), typeof(EntityRepository<>));
            services.AddScoped<IDataProvider, MsSqlDataProvider>();
            services.AddScoped<ISystemMessageService, SystemMessageService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<ISystemSettingService, SystemSettingService>();

            services.AddScoped<IOrderServices, OrderServices>();
            services.AddScoped<IOrderCartService, OrderCartService>();
            services.AddScoped<IDepartmentServices, DepartmentServices>();
            services.AddScoped<DepartmentServices>();


            services.AddHttpContextAccessor();
            services.AddMemoryCache();
        }
    }
}

using EmployeeDAA.Core.Domain.Grid;
using EmployeeDAA.Core.Domain;
using EmployeeDAA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Services.Category
{
    public partial interface ICategoryServices
    {
        Task<Categories> GetByIdAsync(int Id);
        Task InsertAsync(Categories categories);

        Task UpdateAsync(Categories docclass);
        Task DeleteAsync(IList<Categories> categories);
        Task<IList<Categories>> GetByIdAsync(IList<int> ids);
        Task<IPagedList<Categories>> GetAllAsync(GridRequestModel objGrid);
        Task<IList<Categories>> GetAllAsync(bool? isActive = null);
        Task<IList<Categories>> GetAllCategoryAsync(string id = null, bool? isActive = null);


    }
}

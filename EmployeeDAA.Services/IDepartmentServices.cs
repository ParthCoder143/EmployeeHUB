using EmployeeDAA.Core;
using EmployeeDAA.Core.Domain;
using EmployeeDAA.Core.Domain.Department;
using EmployeeDAA.Core.Domain.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Services
{
    public partial interface IDepartmentServices
    {
        Task<Department> GetByIdAsync(int id);

        Task InsertAsync(Department department);

        Task UpdateAsync(Department department);

        Task DeleteAsync(IList<Department> department);
        Task<IPagedList<Department>> GetAllAsync(GridRequestModel objGrid);
        Task<IList<Department>> GetByIdAsync(IList<int> ids);

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmployeeDAA.Core.Domain;
using EmployeeDAA.Core;
using EmployeeDAA.Core.Domain.Grid;

namespace EmployeeDAA.Services.Employee
{
    public partial interface IEmployeeServices
    {
        Task<Employees> GetByIdAsync(int Id);
        Task InsertAsync(Employees employee);
        //Task<string> InsertAsync(string Action, string Name, string Address, string EmailId, string PhoneNo, int UserId, string Username);

        Task UpdateAsync(Employees docclass);
        //Task<string> UpdateAsync(string Action, int Id, string Name, string Address, string EmailId, string PhoneNo, int UserId, string Username);

        Task DeleteAsync(IList<Employees> employees, int UserId, string Username);
        Task<IList<Employees>> GetByIdAsync(IList<int> ids);
        Task<IPagedList<Employees>> GetAllAsync(GridRequestModel objGrid);
    }
}

using DocumentFormat.OpenXml.Office2010.Excel;
using EmployeeDAA.Core;
using EmployeeDAA.Core.Domain;
using EmployeeDAA.Core.Domain.Grid;
using EmployeeDAA.Data;
using LinqToDB.Data;

namespace EmployeeDAA.Services.Employee
{
    public class EmployeeServices : IEmployeeServices
    {
        private readonly IRepository<Employees> _employeeRepository;

        public EmployeeServices(IRepository<Employees> employeeRepository)
        {
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        }

        public virtual async Task<IPagedList<Employees>> GetAllAsync(GridRequestModel objGrid)
        {

            return await _employeeRepository.GetAllPagedAsync(objGrid);
        }
        public Task<Employees> GetByIdAsync(int Id)
        {
            return _employeeRepository.GetByIdAsync(Id);
        }

        public virtual async Task InsertAsync(Employees employee)
        {
            await _employeeRepository.InsertAsync(employee);
        }
        public virtual async Task UpdateAsync(Employees employee)
        {
            await _employeeRepository.UpdateAsync(employee);
        }
        public virtual async Task DeleteAsync(IList<Employees> employees, int UserId, string Username)
        {
            await _employeeRepository.DeleteAsync(employees);
        }


        public virtual async Task<IList<Employees>> GetByIdAsync(IList<int> ids)
        {
            return await _employeeRepository.GetByIdsAsync(ids);
        }

        //public virtual async Task<string> InsertAsync(string Action, string Name, string Address, string EmailId, string PhoneNo, int UserId, string Username)
        //{
        //    try
        //    {
        //        DataParameter[] Param = { new DataParameter("@Action", Action), new DataParameter("@Name", Name), new DataParameter("@Address", Address), new DataParameter("@EmailId", EmailId), new DataParameter("@PhoneNo", PhoneNo) };
        //        await _employeeRepository.InsertAsync("EmployeesStoreProcedure", Param);
        //        return "OK";
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;

        //    }
        //}
        //public virtual async Task<string> UpdateAsync(string Action, int Id, string Name, string Address, string EmailId, string PhoneNo, int UserId, string Username)
        //{
        //    try
        //    {
        //        DataParameter[] Param = { new DataParameter("@Action", Action), new DataParameter("@Id", Id), new DataParameter("@Name", Name), new DataParameter("@Address", Address), new DataParameter("@EmailId", EmailId), new DataParameter("@PhoneNo", PhoneNo) };
        //        await _employeeRepository.UpdateAsync("EmployeesStoreProcedure", Param);
        //        return "OK";
        //    }
        //    catch (Exception ex)
        //    {
        //        return ex.Message;

        //    }
        //}
    }
}
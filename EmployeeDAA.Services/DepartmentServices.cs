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
    public class DepartmentServices : IDepartmentServices

    {
        private readonly IRepository<Department> _DepartmentRepository;
        private readonly IRepository<Employees> _EmployeesRepository;

        public DepartmentServices(IRepository<Department> departmentRepository, IRepository<Employees> employeesRepository = null)
        {
            _DepartmentRepository = departmentRepository;
            _EmployeesRepository = employeesRepository;
        }
        public virtual async Task<IList<Department>> GetByIdAsync(IList<int> ids)
        {
            return await _DepartmentRepository.GetByIdsAsync(ids);
        }

        public virtual async Task DeleteAsync(IList<Department> department)
        {
            await _DepartmentRepository.DeleteAsync(department);
        }


        public virtual async Task<IPagedList<Department>> GetAllAsync(GridRequestModel objGrid)
        {
            IQueryable<Department> query = from d in _DepartmentRepository.Table
                                           join e in _EmployeesRepository.Table on d.EmployeeId equals e.Id
                                           select new Department()
                                           {
                                               Id = d.Id,
                                               EmployeeId = d.EmployeeId,
                                               DepartmentName = d.DepartmentName,
                                               DepartmentCode = d.DepartmentCode,
                                               IsActive = d.IsActive
                                           };
            return await _DepartmentRepository.GetAllPagedAsync(objGrid, query);
        }

        public  Task<Department> GetByIdAsync(int id)
        {
            return _DepartmentRepository.GetByIdAsync(id);
        }

        public virtual async Task InsertAsync(Department department)
        {
            await _DepartmentRepository.InsertAsync(department);
        }

        public virtual async Task UpdateAsync(Department department)
        {
            await _DepartmentRepository.UpdateAsync(department);
        }
    }
}

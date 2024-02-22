using EmployeeDAA.Core;
using EmployeeDAA.Core.Domain.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Services.Settings
{
    public partial interface ISystemSettingService
    {
        Task<IPagedList<Core.Domain.Settings.Settings>> GetAllAsync(GridRequestModel objGrid);
        Task<List<EmployeeDAA.Core.Domain.Settings.Settings>> GetAllAsync();
        Task<EmployeeDAA.Core.Domain.Settings.Settings> GetByIdAsync(int Id);
        Task<Core.Domain.Settings.Settings> GetByKeyAsync(string key);
        Task<IList<EmployeeDAA.Core.Domain.Settings.Settings>> GetByKeysAsync(string[] keys);
        Task UpdateAsync(EmployeeDAA.Core.Domain.Settings.Settings syssetting, int UserId, string Username);



    }
}

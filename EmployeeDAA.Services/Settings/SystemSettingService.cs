using DocumentFormat.OpenXml.Spreadsheet;
using EmployeeDAA.Core.Domain.Grid;
using EmployeeDAA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Services.Settings
{
    public partial class SystemSettingService : ISystemSettingService
    {
        #region Fields

        private readonly IRepository<EmployeeDAA.Core.Domain.Settings.Settings> _syssettingRepository;
        #endregion

        #region Ctor

        public SystemSettingService(IRepository<EmployeeDAA.Core.Domain.Settings.Settings> syssettingRepository)
        {
            _syssettingRepository = syssettingRepository;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets all System Setting
        /// </summary>
        /// <param name="className">System Setting name</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="isActive">
        /// null - load "All"
        /// true - load only "active" System Setting
        /// false - load only "inactive" System Setting
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the System Setting
        /// </returns>
        public virtual async Task<IPagedList<Core.Domain.Settings.Settings>> GetAllAsync(GridRequestModel objGrid)
        {
            return await _syssettingRepository.GetAllPagedAsync(objGrid);
        }

        public virtual async Task<List<EmployeeDAA.Core.Domain.Settings.Settings>> GetAllAsync()
        {
            IList<Core.Domain.Settings.Settings> result = await _syssettingRepository.GetAllAsync(query =>
            {
                return query.Where(x => !string.IsNullOrEmpty(x.Key));
            });
            return result.ToList();
        }

        /// <summary>
        /// Gets a System Setting
        /// </summary>
        /// <param name="Id">System Setting id</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the System Setting
        /// </returns>
        public virtual async Task<EmployeeDAA.Core.Domain.Settings.Settings> GetByIdAsync(int Id)
        {
            return await _syssettingRepository.GetByIdAsync(Id);
        }
        public virtual async Task<EmployeeDAA.Core.Domain.Settings.Settings> GetByKeyAsync(string key)
        {
            IList<Core.Domain.Settings.Settings> sytemsSettings = await _syssettingRepository.GetAllAsync(query => query.Where(x => x.Key == key));
            return sytemsSettings.FirstOrDefault();
        }

        public virtual async Task<IList<EmployeeDAA.Core.Domain.Settings.Settings>> GetByKeysAsync(string[] keys)
        {
            return await _syssettingRepository.GetAllAsync(query => query.Where(x => keys.Contains(x.Key)));
        }

        /// <summary>
        /// Updates the System Setting
        /// </summary>
        /// <param name="syssetting">System Setting</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateAsync(EmployeeDAA.Core.Domain.Settings.Settings syssetting, int UserId, string Username)
        {
            await _syssettingRepository.UpdateAsync(syssetting);
        }
        #endregion
    }
}

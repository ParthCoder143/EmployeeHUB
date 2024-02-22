using EmployeeDAA.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Services.Settings
{
    public partial interface ISystemMessageService
    {
        Task<List<SystemMessage>> GetAllAsync(bool? isActive = null);

    }
}

using EmployeeDAA.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Services.Settings
{
    public partial class SystemMessageService : ISystemMessageService
    {
        private readonly IRepository<SystemMessage> _sysmsgRepository;

        public SystemMessageService(IRepository<SystemMessage> sysmsgRepository)
        {
            _sysmsgRepository = sysmsgRepository;
        }

        public virtual async Task<List<SystemMessage>> GetAllAsync(bool? isActive = null)
        {
            IList<SystemMessage> result = await _sysmsgRepository.GetAllAsync(query =>
            {
                return query.Where(x => x.IsActive);
            });
            return result.ToList();
        }

    }
}

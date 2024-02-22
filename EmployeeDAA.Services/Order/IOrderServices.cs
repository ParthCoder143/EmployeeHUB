using EmployeeDAA.Core.Domain.Grid;
using EmployeeDAA.Core;
using EmployeeDAA.Core.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeDAA.Core.Domain;

namespace EmployeeDAA.Services.Order
{
    public interface IOrderServices
    {
        Task<IPagedList<OrderInfo>> GetAllAsync(GridRequestModel objGrid);

        //Task<List<OrderInfo>> GetByOrder(int OrderId);

        Task InsertAsync(OrderInfo obj);

        //Task InsertAsync(IList<OrderInfo> logs);

        Task UpdateAsync(OrderInfo order);


        Task DeleteAsync(IList<OrderInfo> orders);

        //Task DeleteAsync(IList<OrderInfo> orders);
        Task<IList<OrderInfo>> GetByIdsAsync(IList<int> ids);
        Task<OrderInfo> GetByIdAsync(int id);
    }
}

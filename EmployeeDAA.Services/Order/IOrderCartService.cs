using EmployeeDAA.Core.Domain.Grid;
using EmployeeDAA.Core.Domain.Orders;
using EmployeeDAA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeDAA.Core.Domain;

namespace EmployeeDAA.Services.Order
{
    public interface IOrderCartService
    {
        Task<OrderCart> GetByIdAsync(int Id);

        Task<IPagedList<OrderCart>> GetAllAsync(GridRequestModel objGrid);
        Task InsertAsync(OrderCart orderCart);
        Task UpdateAsync(OrderCart orderCart);
        Task DeleteAsync(IList<OrderCart> cart);
        Task<IList<OrderCart>> GetByIdsAsync(IList<int> ids);
        //Task AddToCartAsync(List<int> productIds);

    }
}

using EmployeeDAA.Core.Domain.Grid;
using EmployeeDAA.Core;
using EmployeeDAA.Core.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB;
using EmployeeDAA.Core.Domain;
using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace EmployeeDAA.Services.Order
{
    public class OrderServices:IOrderServices
    {
        private readonly IRepository<OrderInfo> _ordersRepository;

        public OrderServices(IRepository<OrderInfo> ordersRepository)
        {
            _ordersRepository = ordersRepository;
        }

        public virtual async Task<IPagedList<OrderInfo>> GetAllAsync(GridRequestModel objGrid)
        {

            return await _ordersRepository.GetAllPagedAsync(objGrid);

        }

        public virtual async Task<IList<OrderInfo>> GetByIdsAsync(IList<int> ids)
        {
            return await _ordersRepository.GetByIdsAsync(ids);
        }

        public virtual async Task InsertAsync(OrderInfo obj)
        {
            await _ordersRepository.InsertAsync(obj);
        }
        public virtual async Task UpdateAsync(OrderInfo order)
        {
            await _ordersRepository.UpdateAsync(order);
        }
        public virtual async Task DeleteAsync(IList<OrderInfo> order)
        {
            await _ordersRepository.DeleteAsync(order);
        }



        public virtual async Task<OrderInfo> GetByIdAsync(int id)
        {
            return await _ordersRepository.GetByIdAsync(id);
        }

    }
}

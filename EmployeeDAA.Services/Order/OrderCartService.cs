using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Drawing.Spreadsheet;
using DocumentFormat.OpenXml.Office2010.Excel;
using EmployeeDAA.Core;
using EmployeeDAA.Core.Domain;
using EmployeeDAA.Core.Domain.Grid;
using EmployeeDAA.Core.Domain.Orders;
using LinqToDB;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Services.Order
{

    public class OrderCartService : IOrderCartService
    {
        private readonly IRepository<OrderCart> _orderCartRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<OrderInfo> _orderInfoRepository;
        public OrderCartService(IRepository<OrderCart> orderCartRepository, IRepository<Product> productRepository, IRepository<OrderInfo> orderInfoRepository)
        {
            _orderCartRepository = orderCartRepository;
            _productRepository = productRepository;
            _orderInfoRepository = orderInfoRepository;
        }

        public virtual async Task DeleteAsync(IList<OrderCart> cart)
        {
            await _orderCartRepository.DeleteAsync(cart);
        }



        public virtual async Task<IPagedList<OrderCart>> GetAllAsync(GridRequestModel objGrid)
        {
            IQueryable<OrderCart> query = from d in _orderCartRepository.Table
                                          join f in _productRepository.Table on d.ProductId equals f.Id
                                          join g in _orderInfoRepository.Table on d.OrderId equals g.Id
                                          where !f.IsDeleted
                                          orderby f.SortOrder


                                          select new OrderCart()
                                          {
                                              Id = d.Id,
                                              OrderId = d.OrderId,
                                              ProductId = d.ProductId,
                                              ProductName = f.ProductName,
                                              Quantity = d.Quantity,
                                              UnitPrice = d.UnitPrice,
                                              TotalPrice = d.TotalPrice
                                          };
            return await _orderCartRepository.GetAllPagedAsync(objGrid, query);

        }
        //public virtual async Task AddToCartAsync(List<int> productIds)
        //{

        //    int orderId = await GetOrderIdFromRepository();

        //    foreach (var productId in productIds)
        //    {
        //        var product = await _productRepository.GetByIdAsync(productId);

        //        if (product != null)
        //        {
        //            var orderCart = new OrderCart
        //            {
        //                OrderId = orderId,
        //                ProductId = productId,
        //                ProductName = product.ProductName,
        //                Quantity = 1, 
        //                UnitPrice = product.Price,
        //                TotalPrice = product.Price
        //            };
        //            await _orderCartRepository.InsertAsync(orderCart);
        //        }
        //    }
        //}

        private async Task<int> GetOrderIdFromRepository()
        {
            var latestOrder = await _orderInfoRepository.Table.OrderByDescending(o => o.Id).FirstOrDefaultAsync();

            return latestOrder?.Id ?? 0;
        }

        public Task<OrderCart> GetByIdAsync(int Id)
        {
            return _orderCartRepository.GetByIdAsync(Id);
        }


        public virtual async Task<IList<OrderCart>> GetByIdsAsync(IList<int> ids)
        {
            return await _orderCartRepository.GetByIdsAsync(ids);
        }

        public virtual async Task InsertAsync(OrderCart orderCart)
        {
            await _orderCartRepository.InsertAsync(orderCart);
        }

        public virtual async Task UpdateAsync(OrderCart orderCart)
        {
            await _orderCartRepository.UpdateAsync(orderCart);
        }
    }

}

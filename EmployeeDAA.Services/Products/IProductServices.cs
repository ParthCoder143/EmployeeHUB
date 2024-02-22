using EmployeeDAA.Core.Domain.Grid;
using EmployeeDAA.Core.Domain;
using EmployeeDAA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Spreadsheet;
using EmployeeDAA.Core.Domain.Orders;

namespace EmployeeDAA.Services.Products
{
    public partial interface IProductServices
    {
        Task<Product> GetByIdAsync(int Id);
        Task InsertAsync(Product product);
        Task<IList<Product>> GetByIdsAsync(IList<int> ids);

        Task<IList<Product>> GetAllAsync(bool? isActive = null);

        Task UpdateAsync(Product docclass);
        Task DeleteAsync(IList<Product> products);
        //Task GetProductsByIdsAsync(IList<Product> productIds); 

        Task<IList<Product>> GetByIdAsync(IList<int> ids);
        Task<IPagedList<Product>> GetAllAsync(GridRequestModel objGrid);

    }
}

using EmployeeDAA.Core.Domain.Grid;
using EmployeeDAA.Core.Domain;
using EmployeeDAA.Core;
using EmployeeDAA.Services.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;

namespace EmployeeDAA.Services.Products
{
  
    public class ProductServices : IProductServices
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Categories> _categoryrepository;

        public ProductServices(IRepository<Product> productRepository, IRepository<Categories> categoryrepository)
        {
            _productRepository = productRepository;
            _categoryrepository = categoryrepository;
        }
         public virtual async Task<IList<Product>> GetAllAsync(bool? isActive = null)
        {
            return await _productRepository.GetAllAsync(query =>
            {
                return from d in _productRepository.Table
                       join f in _categoryrepository.Table on d.CategoryId equals f.Id
                       where d.IsDeleted == false
                       select new Product()
                       {
                           Id = d.Id,
                           IsActive = d.IsActive,
                           IsDeleted = d.IsDeleted,
                           ProductName = d.ProductName,
                           Price = d.Price,
                           SortOrder = d.SortOrder,
                           CategoryId = d.CategoryId,
                           CategoryName = f.CategoryName,
                           Photo = d.Photo,
                           CameraUrl = d.CameraUrl,
                          
                       };
            });
        }
        public virtual async Task<IPagedList<Product>> GetAllAsync(GridRequestModel objGrid)
        {
            IQueryable<Product> query = from d in _productRepository.Table
                                             join f in _categoryrepository.Table on d.CategoryId equals f.Id
                                             select new Product()
                                               {
                                                   Id = d.Id,
                                                   IsActive = d.IsActive,
                                                   IsDeleted = d.IsDeleted,
                                                   ProductName = d.ProductName,
                                                 Price = d.Price,
                                                   SortOrder = d.SortOrder,
                                                   CategoryId = d.CategoryId,
                                                   CategoryName = f.CategoryName,
                                                 Photo = d.Photo,
                                                 CameraUrl = d.CameraUrl
                                                  
                          
                                               };
            return await _productRepository.GetAllPagedAsync(objGrid, query);

        }
       
        public Task<Product> GetByIdAsync(int Id)
        {
            return _productRepository.GetByIdAsync(Id);
        }

        public virtual async Task InsertAsync(Product product)
        {
            await _productRepository.InsertAsync(product);
        }

        public virtual async Task UpdateAsync(Product product)
        {
            await _productRepository.UpdateAsync(product);
        }
        public virtual async Task DeleteAsync(IList<Product> product)
        {
            await _productRepository.DeleteAsync(product);
        }


        public virtual async Task<IList<Product>> GetByIdAsync(IList<int> ids)
        {
            return await _productRepository.GetByIdsAsync(ids);
        }

        public virtual async Task<IList<Product>> GetByIdsAsync(IList<int> ids)
        {
            return await _productRepository.GetByIdsAsync(ids);
        }
        //public virtual async Task GetProductsByIdsAsync(IList<Product> product)
        //{
        //    await _productRepository.GetByIdAsync(product);
        //}

    }
}

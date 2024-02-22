using EmployeeDAA.Core.Domain.Grid;
using EmployeeDAA.Core.Domain;
using EmployeeDAA.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Services.Category
{
    public class CategoryServices : ICategoryServices
    {
        private readonly IRepository<Categories> _categoryrepository;
        private readonly IRepository<Product> _productrepo;

        public CategoryServices(IRepository<Categories> categoryrepository, IRepository<Product> productrepo)
        {
            _categoryrepository = categoryrepository ?? throw new ArgumentNullException(nameof(categoryrepository));
            _productrepo = productrepo;
        }

        public Task<Categories> GetByIdAsync(int Id)
        {
            return _categoryrepository.GetByIdAsync(Id);
        }

        public virtual async Task InsertAsync(Categories categories)
        {
            await _categoryrepository.InsertAsync( categories);
        }

        public virtual async Task UpdateAsync(Categories docclass)
        {
            await _categoryrepository.UpdateAsync(docclass);
        }
        public virtual async Task DeleteAsync(IList<Categories> categories)
        {
            await _categoryrepository.DeleteAsync(categories);
        }

        public virtual async Task<IList<Categories>> GetByIdAsync(IList<int> ids)
        {
            return await _categoryrepository.GetByIdsAsync(ids);
        }

        public virtual async Task<IPagedList<Categories>> GetAllAsync(GridRequestModel objGrid)
        {
            return await _categoryrepository.GetAllPagedAsync(objGrid);
        }
        public virtual async Task<IList<Categories>> GetAllAsync(bool? isActive = null)
        {
            return await _categoryrepository.GetAllAsync(query =>
            {
                return query.Where(x => x.IsActive && !x.IsDeleted).OrderBy(x => x.SortOrder);
            });
        }

        public virtual async Task<IList<Categories>> GetAllCategoryAsync(string productId = null, bool? isActive = null)
        {

            var query = from c in _categoryrepository.Table
                        join p in _productrepo.Table on c.Id equals p.CategoryId
                        where productId == null || productId.Contains(p.Id.ToString())
                        select new Categories {
                Id  = c.Id,
                CategoryName = c.CategoryName
            };

            return await _categoryrepository.GetAllAsync(query =>
            {
                return query.Where(x => x.IsActive && !x.IsDeleted).OrderBy(x => x.SortOrder);
            });
        }
    }
}

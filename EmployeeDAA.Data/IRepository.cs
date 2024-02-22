using EmployeeDAA.Core;
using EmployeeDAA.Core.Domain;
using EmployeeDAA.Core.Domain.Grid;
using EmployeeDAA.Core.Domain.Orders;
using EmployeeDAA.Core.Infrastructure;
using LinqToDB.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Services
{
    public partial interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null);
        Task<TEntity> GetByIdAsync(int? id);
        Task UpdateAsync(TEntity entity);
        Task<IList<TEntity>> UpdateAsync(string procedureName, params DataParameter[] parameters);

        Task<TEntity> InsertAsync(TEntity entity);
        Task<IList<TEntity>> InsertAsync(string procedureName, params DataParameter[] parameters);

        Task<IList<TEntity>> GetByIdsAsync(IList<int> ids);
        Task<IPagedList<TEntity>> GetAllPagedAsync(GridRequestModel objGrid, IQueryable<TEntity> query = null, Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null);

        Task UpdateAsync(IList<TEntity> entities);

        Task DeleteAsync(TEntity entity);


        Task DeleteAsync(IList<TEntity> entities);

        IQueryable<TEntity> Table { get; }


    }
}
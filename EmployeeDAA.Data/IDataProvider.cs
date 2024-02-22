using EmployeeDAA.Core;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Services
{
    public partial interface IDataProvider
    {
        //EntityDescriptor GetEntityDescriptor<TEntity>() where TEntity : BaseEntity;
        ITable<TEntity> GetTable<TEntity>() where TEntity : BaseEntity;
        Task<TEntity> InsertEntityAsync<TEntity>(TEntity entity) where TEntity : BaseEntity;

        Task UpdateEntityAsync<TEntity>(TEntity entity) where TEntity : BaseEntity;
        Task<IList<T>> QueryProcAsync<T>(string procedureName, params DataParameter[] parameters);

        Task DeleteEntityAsync<TEntity>(TEntity entity) where TEntity : BaseEntity;

        Task BulkUpdateEntitiesAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity;
        Task BulkDeleteEntitiesAsync<TEntity>(IList<TEntity> entities) where TEntity : BaseEntity;


    }
}

using DocumentFormat.OpenXml.Vml.Office;
using EmployeeDAA.Core;
using EmployeeDAA.Core.Domain;
using EmployeeDAA.Core.Domain.Grid;
using EmployeeDAA.Core.Domain.Orders;
using EmployeeDAA.Data.Extensions;
using LinqToDB;
using LinqToDB.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace EmployeeDAA.Services
{
    public partial class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly IDataProvider _dataProvider;

        public EntityRepository(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }
        public virtual async Task UpdateAsync(IList<TEntity> entities)
        {
            if (!entities.Any())
            {
                return;
            }

            await _dataProvider.BulkUpdateEntitiesAsync(entities);
        }


        public virtual async Task<IPagedList<TEntity>> GetAllPagedAsync(GridRequestModel objGrid, IQueryable<TEntity> query, Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null)
        {
            if (query == null)
            {
                query = Table;
            }

            query = func != null ? func(query) : query;
            return await query.BuildPredicate(objGrid);
        }

        public virtual async Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null)
        {
            async Task<IList<TEntity>> getAllAsync()
            {
                IQueryable<TEntity> query = Table;
                query = func != null ? func(query) : query;

                return await query.ToListAsync();
            }

            return await GetEntitiesAsync(getAllAsync);
        }

        protected virtual async Task<IList<TEntity>> GetEntitiesAsync(Func<Task<IList<TEntity>>> getAllAsync)
        {
            return await getAllAsync();
        }

        public async Task<TEntity> GetByIdAsync(int? id)
        {
            if (!id.HasValue || id == 0)
            {
                return null;
            }

            async Task<TEntity> getEntityAsync()
            {
                return await Table.FirstOrDefaultAsync(entity => entity.Id == Convert.ToInt32(id));
            }

            return await getEntityAsync();
        }
        public virtual IQueryable<TEntity> Table => _dataProvider.GetTable<TEntity>().With("NOLOCK");
        protected virtual IList<TEntity> GetEntities(Func<IList<TEntity>> getAll)
        {
            return getAll();
        }

        public virtual async Task<TEntity> InsertAsync(TEntity entity)
        {
            return await _dataProvider.InsertEntityAsync(entity);
        }

        public virtual async Task UpdateAsync(TEntity entity)
        {
            await _dataProvider.UpdateEntityAsync(entity);
        }
        public virtual async Task<IList<TEntity>> UpdateAsync(string procedureName, params DataParameter[] parameters)
        {
            return await _dataProvider.QueryProcAsync<TEntity>(procedureName, parameters?.ToArray());

        }
        public virtual async Task<IList<TEntity>> InsertAsync(string procedureName, params DataParameter[] parameters)
        {
            return await _dataProvider.QueryProcAsync<TEntity>(procedureName, parameters?.ToArray());

        }

        public virtual async Task<IList<TEntity>> GetByIdsAsync(IList<int> ids)
        {
            if (!ids?.Any() ?? true)
            {
                return new List<TEntity>();
            }

            async Task<IList<TEntity>> getByIdsAsync()
            {
                IQueryable<TEntity> query = Table;

                //get entries
                List<TEntity> entries = await query.Where(entry => ids.Contains(entry.Id)).ToListAsync();

                //sort by passed identifiers
                List<TEntity> sortedEntries = new();
                foreach (int id in ids)
                {
                    TEntity sortedEntry = entries.FirstOrDefault(entry => entry.Id == id);
                    if (sortedEntry != null)
                    {
                        sortedEntries.Add(sortedEntry);
                    }
                }

                return sortedEntries;
            }

            return await getByIdsAsync();
        }
        public virtual async Task DeleteAsync(TEntity entity)
        {
            switch (entity)
            {
                case ISoftDeletedEntity softDeletedEntity:
                    softDeletedEntity.IsDeleted = true;
                    await _dataProvider.UpdateEntityAsync(entity);
                    break;

                default:
                    await _dataProvider.DeleteEntityAsync(entity);
                    break;
            }

            //event notification
            //Log entry
        }
        public virtual async Task DeleteAsync(IList<TEntity> entities)
        {
            if (entities.OfType<ISoftDeletedEntity>().Any())
            {
                // Set Is delete flag true for all ISoftDeletedEntity 
                entities.OfType<ISoftDeletedEntity>().ToList().ConvertAll(x => x.IsDeleted = true);
                await _dataProvider.BulkUpdateEntitiesAsync(entities);
            }
            else
            {
                await _dataProvider.BulkDeleteEntitiesAsync(entities);
            }
        }

       
    }
}

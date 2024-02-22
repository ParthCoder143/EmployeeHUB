using EmployeeDAA.Core.Infrastructure;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.Mapping;
using LinqToDB.Tools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using LinqToDB.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeDAA.Data.Mapping;
using EmployeeDAA.Core;
using Microsoft.Identity.Client;

namespace EmployeeDAA.Data.DataProvider
{
    public abstract class BaseDataProvider
    {
        private MappingSchema GetMappingSchema()
        {
            if (Singleton<MappingSchema>.Instance is null)
            {
                Singleton<MappingSchema>.Instance = new MappingSchema(ConfigurationName)
                {
                    MetadataReader = new FluentMigratorMetadataReader()
                };
            }

            return Singleton<MappingSchema>.Instance;

        }
        public virtual ITable<TEntity> GetTable<TEntity>() where TEntity : BaseEntity
        {
            return new DataContext(LinqToDbDataProvider, GetCurrentConnectionString()) { MappingSchema = GetMappingSchema()}
            .GetTable<TEntity>();
        }

        public virtual async Task<TEntity> InsertEntityAsync<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            using DataConnection dataContext = await CreateDataConnectionAsync();
            entity.Id = await dataContext.InsertWithInt32IdentityAsync(entity);
            return entity;
        }
        public virtual async Task UpdateEntityAsync<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            using DataConnection dataContext = await CreateDataConnectionAsync();
            await dataContext.UpdateAsync(entity);
        }
        public virtual async Task<IList<T>> QueryProcAsync<T>(string procedureName, params DataParameter[] parameters)
        {
            using DataConnection dataContext = await CreateDataConnectionAsync();
            CommandInfo command = new(dataContext, procedureName, parameters);
            List<T> rez = command.QueryProc<T>().ToList();
            UpdateOutputParameters(dataContext, parameters);
            return rez;
        }
        private static void UpdateOutputParameters(DataConnection dataConnection, DataParameter[] parameters)
        {
            if (parameters is null || parameters.Length == 0)
            {
                return;
            }

            foreach (DataParameter dataParam in parameters.Where(p => p.Direction == ParameterDirection.Output))
            {
                UpdateParameterValue(dataConnection, dataParam);
            }
        }
        private static void UpdateParameterValue(DataConnection dataConnection, DataParameter parameter)
        {
            if (dataConnection is null)
            {
                throw new ArgumentNullException(nameof(dataConnection));
            }

            if (parameter is null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            if (dataConnection.Command is IDbCommand command &&
                command.Parameters.Count > 0 &&
                command.Parameters.Contains(parameter.Name) &&
                command.Parameters[parameter.Name] is IDbDataParameter param)
            {
                parameter.Value = param.Value;
            }
        }

        protected virtual async Task<DataConnection> CreateDataConnectionAsync()
        {
            return await CreateDataConnectionAsync(LinqToDbDataProvider);
        }

        protected virtual async Task<DataConnection> CreateDataConnectionAsync(LinqToDB.DataProvider.IDataProvider dataProvider)
        {
            DataConnection dataContext = new(dataProvider, await CreateDbConnectionAsync(), GetMappingSchema())
            {
                CommandTimeout = await DataSettingsManager.GetSqlCommandTimeoutAsync()
            };
            return dataContext;
        }
        protected virtual async Task<IDbConnection> CreateDbConnectionAsync(string connectionString = null)
        {
            DbConnection dbConnection = GetInternalDbConnection(!string.IsNullOrEmpty(connectionString) ? connectionString : await GetCurrentConnectionStringAsync());

            return dbConnection;
        }
        protected static async Task<string> GetCurrentConnectionStringAsync()
        {
            return (await DataSettingsManager.LoadSettingsAsync()).ConnectionString;
        }
        protected static string GetCurrentConnectionString()
        {
            return DataSettingsManager.LoadSettings().ConnectionString;
        }
        protected virtual IDbConnection CreateDbConnection(string connectionString = null)
        {
            DbConnection dbConnection = GetInternalDbConnection(!string.IsNullOrEmpty(connectionString) ? connectionString : GetCurrentConnectionString());

            return dbConnection;
        }
        public virtual async Task DeleteEntityAsync<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            using DataConnection dataContext = await CreateDataConnectionAsync();
            await dataContext.DeleteAsync(entity);
        }
        public virtual async Task BulkUpdateEntitiesAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : BaseEntity
        {
            //we don't use the Merge API on this level, because this API not support all databases.
            //you may see all supported databases by the following link: https://linq2db.github.io/articles/sql/merge/Merge-API.html#supported-databases
            foreach (TEntity entity in entities)
            {
                await UpdateEntityAsync(entity);
            }
        }

        public virtual async Task BulkDeleteEntitiesAsync<TEntity>(IList<TEntity> entities) where TEntity : BaseEntity
        {
            using DataConnection dataContext = await CreateDataConnectionAsync();
            if (entities.All(entity => entity.Id == 0))
            {
                foreach (TEntity entity in entities)
                {
                    await dataContext.DeleteAsync(entity);
                }
            }
            else
            {
                await dataContext.GetTable<TEntity>()
                    .Where(e => e.Id.In(entities.Select(x => x.Id)))
                    .DeleteAsync();
            }
        }

        protected abstract DbConnection GetInternalDbConnection(string connectionString);

        public string ConfigurationName => LinqToDbDataProvider.Name;

        protected abstract LinqToDB.DataProvider.IDataProvider LinqToDbDataProvider { get;  }
    }
}

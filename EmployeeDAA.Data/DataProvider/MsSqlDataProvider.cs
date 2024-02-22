using LinqToDB.DataProvider.SqlServer;
using LinqToDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmployeeDAA.Core;
using LinqToDB.Mapping;
using EmployeeDAA.Services;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;
using LinqToDB.Data;

namespace EmployeeDAA.Data.DataProvider
{
    public partial class MsSqlDataProvider:BaseDataProvider,IDataProvider
    {
        private static readonly Lazy<LinqToDB.DataProvider.IDataProvider> _dataProvider = new(() => new SqlServerDataProvider(ProviderName.SqlServer, SqlServerVersion.v2012, SqlServerProvider.SystemDataSqlClient), true);

        protected override LinqToDB.DataProvider.IDataProvider LinqToDbDataProvider => _dataProvider.Value;

     

        protected override DbConnection GetInternalDbConnection(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException(nameof(connectionString));
            }

            return new SqlConnection(connectionString);
        }
        


    }
}

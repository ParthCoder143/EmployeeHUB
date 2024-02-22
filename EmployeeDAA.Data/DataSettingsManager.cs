using EmployeeDAA.Core.Infrastructure;
using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Data
{
    public static class DataSettingsManager
    {
       public static void IntiDatabaseSettings(IServiceCollection services ,DataSettings DBSettings)
        {
            SetSettings(DBSettings);
            services.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
            .AddSqlServer()
            .WithGlobalConnectionString(DBSettings.ConnectionString)
            .ScanIn(typeof(DataSettings).Assembly).For.Migrations());
        }

        public static void ApplyUpMigrations(IServiceProvider services)
        {
            IFilteringMigrationSource _filteringMigrationSource = services.GetService<IFilteringMigrationSource>();
            IMigrationRunner _migrationRunner = services.GetService<IMigrationRunner>();
            IMigrationRunnerConventions _migrationRunnerConventions = services.GetService<IMigrationRunnerConventions>();

            System.Collections.Generic.IEnumerable<IMigration> migrations = _filteringMigrationSource.GetMigrations(null) ?? Enumerable.Empty<IMigration>();

            IOrderedEnumerable<FluentMigrator.Infrastructure.IMigrationInfo> sortedmigrations = migrations.Select(m => _migrationRunnerConventions.GetMigrationInfoForMigration(m)).OrderBy(migration => migration.Version);

            foreach(FluentMigrator.Infrastructure.IMigrationInfo migrationInfo in sortedmigrations)
            {
                _migrationRunner.MigrateUp(migrationInfo.Version);
            }
        }

        private static void SetSettings(DataSettings DBSettings)
        {
            DataSettings dataSettings = new()
            {
                ConnectionString = DBSettings.ConnectionString,
                LogConnectionString = DBSettings.LogConnectionString,
                SQLCommandTimeout = -1
            };
            Singleton<DataSettings>.Instance = dataSettings;    
        }
        public static DataSettings LoadSettings()
        {
            if (Singleton<DataSettings>.Instance != null)
            {
                return Singleton<DataSettings>.Instance;
            }
            return null;
        }
        public static async Task<DataSettings> LoadSettingsAsync()
#pragma warning restore CS1998 // This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.
        {
            if (Singleton<DataSettings>.Instance != null)
            {
                return Singleton<DataSettings>.Instance;
            }

            return null;
        }
        public partial class DataSettings
        {
            public string ConnectionString { get; set; }
            public string LogConnectionString { get; set; }
            public int? SQLCommandTimeout { get; set; }
        }
        public static async Task<int> GetSqlCommandTimeoutAsync()
        {
            return (await LoadSettingsAsync())?.SQLCommandTimeout ?? -1;
        }
    }
}

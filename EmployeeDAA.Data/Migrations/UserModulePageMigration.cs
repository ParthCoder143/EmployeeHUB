using EmployeeDAA.Core.Domain;
using EmployeeDAA.Core.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Data.Migrations
{
    [MigrationInfo("2024-02-04 09:01:13", "Initialize user module page related Schema")]

    public class UserModulePageMigration : FluentMigrator.Migration
    {
        public override void Up()
        {
            Create
            .Table(nameof(Module)).WithDescription("Module table")
            .WithColumn(nameof(Module.Id)).AsInt32().PrimaryKey().NotNullable().Identity().WithColumnDescription("Auto generated unique identifier")
            .WithColumn(nameof(Module.ModuleName)).AsString(250).NotNullable().WithColumnDescription("Module Name")
            .WithColumn(nameof(Module.IsActive)).AsBoolean().NotNullable().WithColumnDescription("active or not.");

            Create
                    .Table(nameof(Page)).WithDescription("Page table")
                            .WithColumn(nameof(Page.Id)).AsInt32().PrimaryKey().NotNullable().Identity().WithColumnDescription("Auto generated unique identifier")
                            .WithColumn(nameof(Page.ModuleId)).AsInt32().NotNullable().ForeignKey(nameof(Module), nameof(Module.Id)).WithColumnDescription("Module Id")
                            .WithColumn(nameof(Page.PageName)).AsString(250).NotNullable().WithColumnDescription("Page Name")
                            .WithColumn(nameof(Page.PageCode)).AsString(250).NotNullable().WithColumnDescription("Page Code")
                            .WithColumn(nameof(Page.IsShowMenu)).AsBoolean().NotNullable().WithColumnDescription("Show in menu")
                            .WithColumn(nameof(Page.IsShowSearch)).AsBoolean().NotNullable().WithColumnDescription("Show in search in audit table")
                            .WithColumn(nameof(Page.TableNames)).AsCustom("varchar(max)").Nullable().WithColumnDescription("table names with comma separated")
                            .WithColumn(nameof(User.IsActive)).AsBoolean().NotNullable().WithColumnDescription("active or not.");
        }
        public override void Down()
        {
        }
    }

}

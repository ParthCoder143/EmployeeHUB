using EmployeeDAA.Core.Domain.Department;
using EmployeeDAA.Core.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Data.Migrations
{
    [MigrationInfo("2024-09-16 10:27:12", "Add DepartmentCode in Department table")]

    public class AddDepartmentCode : FluentMigrator.Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            if (!Schema.Table(nameof(Department)).Column(nameof(Department.DepartmentCode)).Exists())
            {

                Alter
                  .Table(nameof(Department)).WithDescription("Department")
                  .AddColumn(nameof(Department.DepartmentCode)).AsInt32().NotNullable().WithColumnDescription("Department Code");
            }
            if (!Schema.Table(nameof(Department)).Column(nameof(Department.IsActive)).Exists())
            {
                Alter
                .Table(nameof(Department)).WithDescription("Department")
                .AddColumn(nameof(Department.IsActive)).AsBoolean().NotNullable().WithColumnDescription("Is Active");
            }
        }
    }
}

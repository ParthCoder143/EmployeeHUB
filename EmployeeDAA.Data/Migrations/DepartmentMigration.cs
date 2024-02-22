using EmployeeDAA.Core.Domain;
using EmployeeDAA.Core.Domain.Department;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Data.Migrations
{
    [MigrationInfo("2024-09-16 10:20:12", "Initialize Department Schema")]

    public class DepartmentMigration : FluentMigrator.Migration
    {
        public override void Down()
        {
        }

        public override void Up()
        {
            Create
                           .Table(nameof(Department)).WithDescription("Department")
                           .WithColumn(nameof(Department.Id)).AsInt32().PrimaryKey().NotNullable().Identity().WithColumnDescription("Id is must")
                           .WithColumn(nameof(Department.EmployeeId)).AsInt32().ForeignKey(nameof(Employees), nameof(Employees.Id)).NotNullable().WithColumnDescription("Foreign key referencing the Department table")
                           .WithColumn(nameof(Department.DepartmentName)).AsString(250).Nullable().WithColumnDescription("DepartmentName");
        }
    }
}

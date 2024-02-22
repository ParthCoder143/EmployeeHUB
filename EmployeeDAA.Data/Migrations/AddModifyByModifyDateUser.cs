using EmployeeDAA.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Data.Migrations
{
    [MigrationInfo("2024-02-05 10:05:00", "Add ModifyBy,ModifyDate into User")]

    public class AddModifyByModifyDateUser:FluentMigrator.Migration
    {
        public override void Up()
        {
            Alter
                .Table(nameof(User))
              .AddColumn(nameof(User.ModifyBy)).AsInt32().Nullable().WithColumnDescription("Modify By")
            .AddColumn(nameof(User.ModifyDate)).AsDateTime().Nullable().WithColumnDescription("Modify Date");

        }
        public override void Down()
        {

        }
    }
}

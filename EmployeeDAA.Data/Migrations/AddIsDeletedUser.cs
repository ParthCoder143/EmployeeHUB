using EmployeeDAA.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Data.Migrations
{
    [MigrationInfo("2024-02-05 10:00:00", "Add IsDeleted into User")]

    public class AddIsDeletedUser:FluentMigrator.Migration
    {

        public override void Up()
        {
            if (!Schema.Table(nameof(User)).Column(nameof(User.IsDeleted)).Exists())
            {
                Alter
                    .Table(nameof(User))
                        .AddColumn(nameof(User.IsDeleted)).AsBoolean().NotNullable().WithColumnDescription("For delete flag");
            }
        }
        public override void Down()
        {

        }

    }
}

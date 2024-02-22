using EmployeeDAA.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Data.Migrations
{
    [MigrationInfo("2024-02-06 11:17:12", "Add UserTypeId into User")]

    public class AddUserTypeIdUser : FluentMigrator.Migration
    {
        public override void Up()
        {
            if (!Schema.Table(nameof(User)).Column(nameof(User.UserTypeId)).Exists())
            {
                Alter
                    .Table(nameof(User))
                .AddColumn(nameof(User.UserTypeId)).AsInt32().Nullable().WithColumnDescription("User type id");
            }
        }
        public override void Down()
        {
        }

      
    }
}

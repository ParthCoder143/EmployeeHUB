using DocumentFormat.OpenXml.Wordprocessing;
using EmployeeDAA.Core.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Data.Migrations
{
    [MigrationInfo("2024-02-04 20:28:00", "Add Auto Filed into document type")]

    public class AddPageIdPermission:FluentMigrator.Migration
    {
        public override void Up()
        {
            if (!Schema.Table(nameof(Permission)).Column(nameof(Permission.PageId)).Exists())
            {
                Alter
                    .Table(nameof(Permission))
                .AddColumn(nameof(Permission.PageId)).AsInt32().NotNullable().ForeignKey(nameof(Page), nameof(Page.Id)).WithColumnDescription("Role Id");
            }
        }
        public override void Down()
        {

        }
    }
}

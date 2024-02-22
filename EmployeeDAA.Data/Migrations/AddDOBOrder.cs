using EmployeeDAA.Core.Domain;
using EmployeeDAA.Core.Domain.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Data.Migrations
{
    [MigrationInfo("2024-02-08 21:00:00", "Initialize DOB related DateTime")]

    public class AddDOBOrder: FluentMigrator.Migration
    {
        public override void Down()
        {
            throw new NotImplementedException();
        }

        public override void Up()
        {
            //if (!Schema.Table(nameof(OrderInfo)).Column(nameof(OrderInfo.DateOfBirth)).Exists())
            //{
            //    Alter
            //       .Table(nameof(OrderInfo))
            //   .AddColumn(nameof(OrderInfo.DateOfBirth)).AsDateTime().Nullable();
            //}
            //if (!Schema.Table(nameof(OrderInfo)).Column(nameof(OrderInfo.IsDeleted)).Exists())
            //{
            if (!Schema.Table(nameof(OrderInfo)).Column(nameof(OrderInfo.IsDeleted)).Exists())
            {
                Alter
                       .Table(nameof(OrderInfo))
                       .AddColumn(nameof(OrderInfo.IsDeleted)).AsBoolean().Nullable().WithColumnDescription("For delete flag");
            }
            //}        }
        }
    }
}

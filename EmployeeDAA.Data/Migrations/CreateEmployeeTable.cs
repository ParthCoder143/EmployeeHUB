
using FluentMigrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeDAA.Data.Migrations
{
   
    [MigrationInfo("2024-01-29 09:17:12", "Initialize base Schema")]

    public class CreateEmployeeTable:Migration
    {
        public override void Up()
        {
            Create.Table("Employees")
               .WithColumn("Id").AsInt32().NotNullable().PrimaryKey().Identity()
               .WithColumn("Name").AsString(255).NotNullable()
               .WithColumn("Address").AsString(255).NotNullable()
               .WithColumn("EmailId").AsString(255).NotNullable()
               .WithColumn("PhoneNo").AsString(255).NotNullable();
        }

        public override void Down() 
        {
        
        }
    }
}

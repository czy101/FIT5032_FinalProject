namespace Test3.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addsomeattribute : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Person_name", c => c.String());
            AddColumn("dbo.AspNetUsers", "Age", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "Gender", c => c.String());
            AddColumn("dbo.AspNetUsers", "Person_role", c => c.String());
            AddColumn("dbo.AspNetUsers", "Department", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Department");
            DropColumn("dbo.AspNetUsers", "Person_role");
            DropColumn("dbo.AspNetUsers", "Gender");
            DropColumn("dbo.AspNetUsers", "Age");
            DropColumn("dbo.AspNetUsers", "Person_name");
        }
    }
}

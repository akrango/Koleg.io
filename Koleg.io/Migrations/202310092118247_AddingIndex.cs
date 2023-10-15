namespace Koleg.io.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddingIndex : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Index", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Index");
        }
    }
}

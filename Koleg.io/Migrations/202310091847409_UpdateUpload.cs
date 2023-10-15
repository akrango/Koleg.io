namespace Koleg.io.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUpload : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Uploads", "Description", c => c.String());
            DropColumn("dbo.Uploads", "Descrition");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Uploads", "Descrition", c => c.String());
            DropColumn("dbo.Uploads", "Description");
        }
    }
}

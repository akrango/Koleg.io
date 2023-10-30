namespace Koleg.io.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UploadNotidication : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Uploads", "IsCommentedOn", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Uploads", "IsCommentedOn");
        }
    }
}

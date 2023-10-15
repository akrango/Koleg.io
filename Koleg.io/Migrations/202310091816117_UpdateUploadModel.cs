namespace Koleg.io.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUploadModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Uploads", "UserId", c => c.Int(nullable: false));
            AddColumn("dbo.Uploads", "User_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Uploads", "User_Id");
            AddForeignKey("dbo.Uploads", "User_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Uploads", "User_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Uploads", new[] { "User_Id" });
            DropColumn("dbo.Uploads", "User_Id");
            DropColumn("dbo.Uploads", "UserId");
        }
    }
}

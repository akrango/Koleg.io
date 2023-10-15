namespace Koleg.io.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUploadModel1 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Uploads", new[] { "User_Id" });
            DropColumn("dbo.Uploads", "UserId");
            RenameColumn(table: "dbo.Uploads", name: "User_Id", newName: "UserId");
            AlterColumn("dbo.Uploads", "UserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Uploads", "UserId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Uploads", new[] { "UserId" });
            AlterColumn("dbo.Uploads", "UserId", c => c.Int(nullable: false));
            RenameColumn(table: "dbo.Uploads", name: "UserId", newName: "User_Id");
            AddColumn("dbo.Uploads", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Uploads", "User_Id");
        }
    }
}

namespace Koleg.io.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteComments : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "UploadId", "dbo.Uploads");
            DropForeignKey("dbo.Comments", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Comments", new[] { "UploadId" });
            DropIndex("dbo.Comments", new[] { "UserId" });
            DropTable("dbo.Comments");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UploadId = c.Int(nullable: false),
                        UserId = c.String(maxLength: 128),
                        CommentText = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.Comments", "UserId");
            CreateIndex("dbo.Comments", "UploadId");
            AddForeignKey("dbo.Comments", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Comments", "UploadId", "dbo.Uploads", "Id", cascadeDelete: true);
        }
    }
}

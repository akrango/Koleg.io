namespace Koleg.io.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Comments : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Comments",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UploadId = c.Int(nullable: false),
                        UserId = c.String(),
                        CommentText = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Uploads", t => t.UploadId, cascadeDelete: true)
                .Index(t => t.UploadId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Comments", "UploadId", "dbo.Uploads");
            DropIndex("dbo.Comments", new[] { "UploadId" });
            DropTable("dbo.Comments");
        }
    }
}

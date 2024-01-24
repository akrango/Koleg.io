namespace Koleg.io.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changes : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UploadChunks", "FileId", "dbo.Uploads");
            DropIndex("dbo.UploadChunks", new[] { "FileId" });
            DropTable("dbo.UploadChunks");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UploadChunks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ChunkData = c.Binary(),
                        FileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateIndex("dbo.UploadChunks", "FileId");
            AddForeignKey("dbo.UploadChunks", "FileId", "dbo.Uploads", "Id", cascadeDelete: true);
        }
    }
}

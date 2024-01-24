namespace Koleg.io.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUpload1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Uploads", "NumberOfRatingVotes", c => c.Int(nullable: false));
            AddColumn("dbo.Uploads", "TotalSumOfRatings", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Uploads", "TotalSumOfRatings");
            DropColumn("dbo.Uploads", "NumberOfRatingVotes");
        }
    }
}

namespace Koleg.io.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UploadUserPicture : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ProfilePicturePath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ProfilePicturePath");
        }
    }
}

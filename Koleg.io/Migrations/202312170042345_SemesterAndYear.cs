namespace Koleg.io.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SemesterAndYear : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Subjects", "Semester", c => c.String(nullable: false));
            AddColumn("dbo.Subjects", "Year", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Subjects", "Year");
            DropColumn("dbo.Subjects", "Semester");
        }
    }
}

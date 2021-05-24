namespace ASP.NET.MVC.Demo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataAnnotations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Movies", "Title", c => c.String(maxLength: 60));
            AlterColumn("dbo.Movies", "Rating", c => c.String(maxLength: 5));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Movies", "Rating", c => c.String());
            AlterColumn("dbo.Movies", "Title", c => c.String());
        }
    }
}

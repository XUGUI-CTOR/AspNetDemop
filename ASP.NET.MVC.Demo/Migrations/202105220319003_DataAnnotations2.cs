namespace ASP.NET.MVC.Demo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DataAnnotations2 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Movies", "Genre", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Movies", "Genre", c => c.String());
        }
    }
}

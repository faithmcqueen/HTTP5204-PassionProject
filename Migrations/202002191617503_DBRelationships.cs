namespace BookClubMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DBRelationships : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Reviews", "BookID", c => c.Int(nullable: false));
            CreateIndex("dbo.Reviews", "BookID");
            AddForeignKey("dbo.Reviews", "BookID", "dbo.Books", "BookID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "BookID", "dbo.Books");
            DropIndex("dbo.Reviews", new[] { "BookID" });
            DropColumn("dbo.Reviews", "BookID");
        }
    }
}

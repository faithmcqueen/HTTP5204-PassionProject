namespace BookClubMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class String : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BookClubs", "MeetDay", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.BookClubs", "MeetDay");
        }
    }
}

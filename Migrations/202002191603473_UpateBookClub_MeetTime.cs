namespace BookClubMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpateBookClub_MeetTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.BookClubs", "MeetTime", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.BookClubs", "MeetTime", c => c.DateTime(nullable: false));
        }
    }
}

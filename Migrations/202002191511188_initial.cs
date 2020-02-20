namespace BookClubMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookClubs",
                c => new
                    {
                        ClubID = c.Int(nullable: false, identity: true),
                        ClubName = c.String(),
                        MeetTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ClubID);
            
            CreateTable(
                "dbo.Books",
                c => new
                    {
                        BookID = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Author = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.BookID);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        MemberID = c.Int(nullable: false, identity: true),
                        FisrtName = c.String(),
                        LastName = c.String(),
                        ClubID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MemberID)
                .ForeignKey("dbo.BookClubs", t => t.ClubID, cascadeDelete: true)
                .Index(t => t.ClubID);
            
            CreateTable(
                "dbo.Reviews",
                c => new
                    {
                        ReviewID = c.Int(nullable: false, identity: true),
                        ReviewContent = c.String(),
                        MemberID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReviewID)
                .ForeignKey("dbo.Members", t => t.MemberID, cascadeDelete: true)
                .Index(t => t.MemberID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reviews", "MemberID", "dbo.Members");
            DropForeignKey("dbo.Members", "ClubID", "dbo.BookClubs");
            DropIndex("dbo.Reviews", new[] { "MemberID" });
            DropIndex("dbo.Members", new[] { "ClubID" });
            DropTable("dbo.Reviews");
            DropTable("dbo.Members");
            DropTable("dbo.Books");
            DropTable("dbo.BookClubs");
        }
    }
}

namespace BookClubMVC.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bookxclub : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BookxClubs",
                c => new
                    {
                        BooksxClubsID = c.Int(nullable: false, identity: true),
                        BookID = c.Int(nullable: false),
                        BookClubID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.BooksxClubsID)
                .ForeignKey("dbo.BookClubs", t => t.BookClubID, cascadeDelete: true)
                .ForeignKey("dbo.Books", t => t.BookID, cascadeDelete: true)
                .Index(t => t.BookID)
                .Index(t => t.BookClubID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BookxClubs", "BookID", "dbo.Books");
            DropForeignKey("dbo.BookxClubs", "BookClubID", "dbo.BookClubs");
            DropIndex("dbo.BookxClubs", new[] { "BookClubID" });
            DropIndex("dbo.BookxClubs", new[] { "BookID" });
            DropTable("dbo.BookxClubs");
        }
    }
}

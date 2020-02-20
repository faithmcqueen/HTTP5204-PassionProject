using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BookClubMVC.Data
{
    public class BookClubContext : DbContext
    {
        public BookClubContext() : base("name=BookClubContext")
        {
        }

        public System.Data.Entity.DbSet<BookClubMVC.Models.Book> Books { get; set; }
        public System.Data.Entity.DbSet<BookClubMVC.Models.BookClub> BookClubs { get; set; }
        public System.Data.Entity.DbSet<BookClubMVC.Models.Member> Members { get; set; }
        public System.Data.Entity.DbSet<BookClubMVC.Models.Review> Reviews { get; set; }

        public System.Data.Entity.DbSet<BookClubMVC.Models.BookxClub> BooksxClubs { get; set; }

    }
}
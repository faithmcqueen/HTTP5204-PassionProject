using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookClubMVC.Models.ViewModels
{
    public class UpdateClub
    {
        //access information about one specific club
        public virtual BookClub bookclub { get; set; }

        //access information from booksxclubs
        public List<BookxClub> booksxclubs { get; set; }

        public List<Book> books { get; set; }
        public virtual Book book { get; set; }
    }
}
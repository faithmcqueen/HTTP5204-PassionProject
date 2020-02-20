using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookClubMVC.Models.ViewModels
{
    public class ShowBookClub
    {

        //access info about one book club
        public virtual BookClub bookclub { get; set; }
        
        //access information about multiple members of the book club
        public List<Member> members { get; set; }
        public virtual Member member { get; set; }

        //access information about multiple books that the book club is reading
        public List<Book> books { get; set; }
        public virtual BookxClub bookxclubs { get; set; }

    }
}
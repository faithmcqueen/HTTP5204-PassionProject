using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookClubMVC.Models.ViewModels
{
    public class NewReview
    {
        //information about review
        public virtual Review review { get; set; }

        //information about members
        public List<Member> members { get; set; }
        public virtual Member member { get; set; }

        public List<Book> books { get; set; }
        public virtual Book book { get; set; }

    }
}
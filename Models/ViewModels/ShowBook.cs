using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookClubMVC.Models.ViewModels
{
    public class ShowBook
    {

        //information about each book
        public virtual Book book { get; set; }

        //information for reviews
        public List<Review> reviews { get; set; }
        //information about members
        public virtual List<Member> reviewmembers { get; set; }
        public virtual Member reviewmember { get; set; }

    }
}
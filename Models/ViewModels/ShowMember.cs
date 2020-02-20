using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookClubMVC.Models.ViewModels
{
    public class ShowMember
    {
        //access information about book club member
        public virtual Member member { get; set; }

        //access information about reviews left by members
        public List<Review> reviews { get; set; }

        //access information about reviews left by members
        public List<Book> books { get; set; }

        //access information about book club the member belongs to
        public virtual BookClub bookclub { get; set; }
    }
}
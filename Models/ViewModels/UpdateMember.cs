using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookClubMVC.Models.ViewModels
{
    public class UpdateMember
    {
        //access information about one specific member
        public Member Member { get; set; }

        //access book clubs member can join
        public List<BookClub> bookclubs { get; set; }
    }
}
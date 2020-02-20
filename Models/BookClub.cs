using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookClubMVC.Models
{
    public class BookClub
    {

        /* A book club is a group of members that meet to discuss specific groups that they have agreed to read
         * The information that book clubs include is:
         * - Club ID 
         * - Club name
         * - Meeting time
         */

        [Key]
        public int ClubID { get; set; }
        public string ClubName { get; set; }
        public string MeetDay { get; set; }
        public string MeetTime { get; set; }

        //Representing the relationship: one club can have many members:
        public ICollection<Member> Members { get; set; }

    }
}
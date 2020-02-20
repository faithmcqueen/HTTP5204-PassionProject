using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookClubMVC.Models
{
    public class Review
    {
        /* A review is a block of text that a Member will leave about a specific Book, giving their opinion and thoughts on the books. Reviews reference:
         * - Review ID
         * - Review Content
         * - Member ID (FK)
         */

        [Key]
        public int ReviewID { get; set; }
        public string ReviewContent { get; set; }
        //One member can leave Many reviews; represent the "many" with a foreign key in the table referencing the members table
        public int MemberID { get; set; }
        [ForeignKey("MemberID")]
        public virtual Member Members { get; set; }

        //One book can have many reviews. To represent the "Many" reviews, we need a foreign key in the table referencing the books table
        public int BookID { get; set; }
        [ForeignKey("BookID")]
        public virtual Book Books { get; set; }
    }
}
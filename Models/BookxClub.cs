using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookClubMVC.Models
{
    public class BookxClub
    {
        /* This is a bridging table to connect the books and the book clubs. The data that this table pulls includes
         * - BooksxReviews ID 
         * - Book ID (FK)
         * - BookClubID (FK)
         */

        [Key]
        public int BooksxClubsID { get; set; }
        public int BookID { get; set; }
        [ForeignKey("BookID")]
        public virtual Book Books { get; set; }
        public int BookClubID { get; set; }
        [ForeignKey("BookClubID")]
        public virtual BookClub BookClubs { get; set; }


    }
}
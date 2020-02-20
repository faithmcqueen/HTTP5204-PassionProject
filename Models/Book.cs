using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BookClubMVC.Models
{
    public class Book
    {
        /* A book is something that a Book Club agrees to read, and members will leave reviews for. Many books can be read by many users. 
         * Things that describe books are:
         * - Title
         * - Author
         * - Description
         */

        [Key]
        public int BookID { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }

        /* Many users can read man books. Representing the "many" books: */
        public ICollection<Review> Reviews { get; set; }

    }
}
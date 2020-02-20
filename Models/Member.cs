using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace BookClubMVC.Models
{
    public class Member
    {
        /* A member is a person who belongs to a book club and regularly attends meetings. As a member of the book club, they read the books that the club is reading, and will leave reviews about each book. A member includes:
       * - Member ID
       * - First Name
       * - Last Name
       * - Club ID (FK)
       */

        [Key]
        public int MemberID { get; set; }
        public string FisrtName { get; set; }
        public string LastName { get; set; }
        /* Many members can belong to one club. We need to  represent the Many in that relationship by referencing the ClubID from the BookClubs table */
        public int ClubID { get; set; }
        [ForeignKey("ClubID")]
        public virtual BookClub BookClubs { get; set; }

        //Representing the many in the relationship: one member can leave many reviews
        public ICollection<Review> Reviews { get; set; }

    }
}
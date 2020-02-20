using System;
using System.Collections.Generic;
using System.Data;
//required for SqlParameter class
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BookClubMVC.Data;
using BookClubMVC.Models;
//using BookClubMVC.Models.ViewModels;
using System.Diagnostics;
using System.IO; //necessary for image uploading!!!!
using BookClubMVC.Models.ViewModels;

namespace BookClubMVC.Controllers
{
    public class ReviewController : Controller
    {

        /*This controller handles all of the methods for our Review views - including:
         * New()
         * Update()
         * Delete() and DeleteConfirm()
         * We do not have a List() or Show() method for reviews, as the reviews are listed on the Book > Show() method
         */

        //Call new database connection for all logic
        private BookClubContext db = new BookClubContext();
        public ActionResult New()
        {
            //Pull the Book data to add to our New() method for members
            List<Book> books = db.Books.SqlQuery("select * from Books").ToList();
            List<Member> members = db.Members.SqlQuery("select * from Members").ToList();

            NewReview NewReviewViewModel = new NewReview();
            NewReviewViewModel.books = books;
            NewReviewViewModel.members = members;

            return View(NewReviewViewModel);
        }

        //What happes when we submit the New form? We need a HttpPost to make this work
        //We need to pass through both the values from the members table but also the values from the club table
        [HttpPost]
        public ActionResult New(string ReviewContent, int MemberID, int BookID)
        {
            // decalre what our query is going to be by pulling the name for each form input on the New view file
            string query = "Insert into Reviews (ReviewContent, BookID, MemberID) values (@ReviewContent, @BookID, @MemberID)";

            //create SQL parameters array so that we can pass through the correct values, and protect from SQL injection
            SqlParameter[] sqlparams = new SqlParameter[3];
            sqlparams[0] = new SqlParameter("@ReviewContent", ReviewContent);
            sqlparams[1] = new SqlParameter("@MemberID", MemberID);
            sqlparams[2] = new SqlParameter("@BookID", BookID);

            //Write a debug line so we can be sure that we are pulling the correct data
            Debug.WriteLine("Trying to leave a new review for" + BookID + " by " + MemberID + " that says " + ReviewContent);

            //Execute the DB command to add a new member
            db.Database.ExecuteSqlCommand(query, sqlparams);

            return Redirect("/Book/Show/" + BookID);

        }

        //SET: Update - get the data
        public ActionResult Update(int id)
        {
            Review selectedreview = db.Reviews.SqlQuery("select * from Reviews where ReviewID = @id", new SqlParameter("@id", id)).FirstOrDefault();
            List<Book> books = db.Books.SqlQuery("select * from Books").ToList();
            List<Member> members = db.Members.SqlQuery("select * from Members").ToList();

            UpdateReview UpdateReviewViewModel = new UpdateReview();
            UpdateReviewViewModel.review = selectedreview;
            UpdateReviewViewModel.books = books;
            UpdateReviewViewModel.members = members;
            
            return View(UpdateReviewViewModel);
        }

        //SET: make the update
        [HttpPost]
        public ActionResult Update(int id, string ReviewContent, int MemberID, int BookID)
        {
            //create the sql query that will update the record
            string query = "update Reviews set ReviewContent = @ReviewContent, MemberID = @MemberID, BookID = @BookID where ReviewID = @id";
            //name the parameters that we are passing through here
            SqlParameter[] sqlparams = new SqlParameter[4];
            sqlparams[0] = new SqlParameter("@ReviewContent", ReviewContent);
            sqlparams[1] = new SqlParameter("@MemberID", MemberID);
            sqlparams[2] = new SqlParameter("@BookID", BookID);
            sqlparams[3] = new SqlParameter("@id", id);

            //Execute the Database query to update the record
            db.Database.ExecuteSqlCommand(query, sqlparams);

            //When this is done, user should return back to the Show Book page
            return Redirect("/Book/Show/" + BookID);
        }

        //DELETE a review from the book
        public ActionResult DeleteConfirm(int id)
        {
            //get one specific review
            string query = "select * from Reviews where ReviewID = @id";
            //create parameter to prevent SQL injections
            SqlParameter param = new SqlParameter("@id", id);
            //execute the query
            Review selectedreview = db.Reviews.SqlQuery(query, param).FirstOrDefault();

            return View(selectedreview);
        }
        //Action for once the DeleteConfirm form has been submitted
        [HttpPost]
        public ActionResult Delete(int id)
        {
            //Create the SQL query
            string query = "delete from Reviews where ReviewID = @id";
            //Parameter for security
            SqlParameter param = new SqlParameter("@id", id);
            //execute the database query
            db.Database.ExecuteSqlCommand(query, param);

            return Redirect("/Book/List");
        }
    }
}
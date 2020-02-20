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
    public class MemberController : Controller
    {

        /*This controller handles all of the methods for our Member views - including:
         * Show()
         * New()
         * Update()
         * Delete() and DeleteConfirm()
         * We do not have a List() method for members, as the members are listed on the BookClub > Show() method
         */

        //Call new database connection for all logic
        private BookClubContext db = new BookClubContext();

        //SET a new member
        //First: view the page
        public ActionResult New()
        {
            //Pull the Book Club data to add to our New() method for members
            List<BookClub> bookclubs = db.BookClubs.SqlQuery("select * from BookClubs").ToList();
            return View(bookclubs);
        }

        //What happes when we submit the New form? We need a HttpPost to make this work
        //We need to pass through both the values from the members table but also the values from the club table
        [HttpPost]
        public ActionResult New(string FisrtName, string LastName, int ClubID)
        {
            // decalre what our query is going to be by pulling the name for each form input on the New view file
            string query = "Insert into members (FisrtName, LastName, ClubID) values (@FisrtName, @LastName, @ClubID)";

            //create SQL parameters array so that we can pass through the correct values, and protect from SQL injection
            SqlParameter[] sqlparams = new SqlParameter[3];
            sqlparams[0] = new SqlParameter("@FisrtName", FisrtName);
            sqlparams[1] = new SqlParameter("@LastName", LastName);
            sqlparams[2] = new SqlParameter("@ClubID", ClubID);

            //Write a debug line so we can be sure that we are pulling the correct data
            Debug.WriteLine("Trying to create a new member named" + FisrtName + " " + LastName + " in club " + ClubID);

            //Execute the DB command to add a new member
            db.Database.ExecuteSqlCommand(query, sqlparams);

            return Redirect("/BookClub/List");

        }

        //SET new data with Update method
        //view the data in the update form for one specific member

        public ActionResult Update(int id)
        {
            Member selectedmember = db.Members.SqlQuery("select * from Members where MemberID = @id", new SqlParameter("@id", id)).FirstOrDefault();
            //get book club data to pull into dropdown
            List<BookClub> BookClubs = db.BookClubs.SqlQuery("select * from BookClubs").ToList();

            //connect data to ViewModel
            UpdateMember UpdateMemberViewModel = new UpdateMember();
            UpdateMemberViewModel.Member = selectedmember;
            UpdateMemberViewModel.bookclubs = BookClubs;

            //Display the data in the page
            return View(UpdateMemberViewModel);
        }

        //SET: When the form submits, the data needs to be sent to the database and the updates need to be recorded. This is done through HttpPost.
        [HttpPost]
        public ActionResult Update(int id, string FisrtName, string LastName, int ClubID)
        {
            //create the sql query that will update the record
            string query = "update Members set FisrtName = @FisrtName, LastName = @LastName, ClubID = @ClubID where MemberID = @id";
            //name the parameters that we are passing through here
            SqlParameter[] sqlparams = new SqlParameter[4];
            sqlparams[0] = new SqlParameter("@FisrtName", FisrtName);
            sqlparams[1] = new SqlParameter("@LastName", LastName);
            sqlparams[2] = new SqlParameter("@ClubID", ClubID);
            sqlparams[3] = new SqlParameter("@id", id);

            //Execute the Database query to update the record
            db.Database.ExecuteSqlCommand(query, sqlparams);

            //When this is done, user should return back to the Member page
            return Redirect("/Member/Show/" + id);
        }

        //DELETE a record from the system
        //Get one single record and display any data on the DeleteConfirm page
        public ActionResult DeleteConfirm(int id)
        {
            //get one specific member
            string query = "select * from Members where MemberID = @id";
            //create parameter to prevent SQL injections
            SqlParameter param = new SqlParameter("@id", id);
            //execute the query
            Member selectedmember = db.Members.SqlQuery(query, param).FirstOrDefault();

            return View(selectedmember);
        }
        //Action for once the DeleteConfirm form has been submitted
        [HttpPost]
        public ActionResult Delete(int id)
        {
            //Create the SQL query
            string query = "delete from Members where MemberID = @id";
            //Parameter for security
            SqlParameter param = new SqlParameter("@id", id);
            //execute the database query
            db.Database.ExecuteSqlCommand(query, param);

            return Redirect("/BookClub/List");
        }

        //GET: Show the member details
        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Create a query to get all of the data for one specific member
            Member Member = db.Members.SqlQuery("select * from Members where MemberID=@MemberID", new SqlParameter("@MemberID", id)).FirstOrDefault();
            //if there is no member with that id, display error
            if (Member == null)
            {
                return HttpNotFound();
            }

            //Create a join to get the data we need about the book clubs
            string query = "select * from BookClubs inner join Members on BookClubs.ClubID = Members.ClubID where Members.MemberID = @id";
            SqlParameter param = new SqlParameter("@id", id);
            BookClub BookClubs = db.BookClubs.SqlQuery(query, param).FirstOrDefault();

            //Create a join to get the data we need about the member reviews
            string query2 = "select * from Reviews inner join Members on Reviews.MemberID = Members.MemberID where Members.MemberID = @id";
            SqlParameter param2 = new SqlParameter("@id", id);
            List<Review> MemberReviews = db.Reviews.SqlQuery(query2, param2).ToList();


            ShowMember viewmodel = new ShowMember();
            viewmodel.member = Member;
            viewmodel.bookclub = BookClubs;
            viewmodel.reviews = MemberReviews;

            return View(viewmodel);
        }
    }
}
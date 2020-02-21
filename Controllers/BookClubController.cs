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
    public class BookClubController : Controller
    {

        /*This controller handles all of the methods for our Member views - including:
         * List()
         * Show()
         * New()
         * Update()
         * Delete() and DeleteConfirm()
         */

        //Call new database connection for all logic
        private BookClubContext db = new BookClubContext();

        //GET List view of all book clubs

        public ActionResult List(string clubsearch)
        {
            //create a debug line to see if we can access the search query user is trying to use
            Debug.WriteLine("The search term is " + clubsearch);

            //declare the SQL query in a variable to use with search term
            string query = "Select * from BookClubs";

            //take search term and add it to our query in an if statement

            if (clubsearch != "")
            {
                //concat our search query with sql query
                query = query + " where ClubName like '%" + @clubsearch + "%'";
                SqlParameter param = new SqlParameter("@clubsearch", clubsearch);

                //Call our debug line to show what our SQL query was and confirm accuracy
                Debug.WriteLine("Our query is " + query);
            } 

            //Display results of search in a list
            List<BookClub> bookclubs = db.BookClubs.SqlQuery(query).ToList();
            return View(bookclubs);
        }

        //GET: Show individual Book Club
        public ActionResult Show(int? id)
        {
            //make sure that there is an ID selected to navigate to BookClub/Show/[id]
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Make our database connection with for Book model
            // Create a paramaterized SQL query to bring up information for one book where our BookIDs match.
            BookClub BookClub = db.BookClubs.SqlQuery("Select * from BookClubs where ClubID=@ClubID", new SqlParameter("@ClubID", id)).FirstOrDefault();

            // Create an error message for if there is no book for current ID (an invalid ID)
            if (BookClub == null)
            {
                return HttpNotFound();
            }

            //We need to pull member information for each individual book club
            string query = "select * from Members inner join BookClubs on Members.ClubID = BookClubs.ClubID where BookClubs.ClubID = @id";
            string query2 = "select * from Books inner join BookxClubs on BookxClubs.BookID = Books.BookID where BookxClubs.BookClubID = @id";
            //create SQL Parameter to prevent SQL injections
            SqlParameter param = new SqlParameter("@id", id);
            SqlParameter param2 = new SqlParameter("@id", id);
            List<Member> Members = db.Members.SqlQuery(query, param).ToList();
            List<Book> Books = db.Books.SqlQuery(query2, param2).ToList();

            Models.ViewModels.ShowBookClub viewmodel = new Models.ViewModels.ShowBookClub();
            viewmodel.bookclub = BookClub;
            viewmodel.members = Members;
            viewmodel.books = Books;

            return View(viewmodel);
        }

        public ActionResult New()
        {
            return View();
        }

        //SET: Add a new book club
        [HttpPost]
        public ActionResult New(string ClubName, string MeetDay, string MeetTime)
        {
            // decalre what our query is going to be by pulling the name for each form input on the New.cshtml page
            string query = "Insert into BookClubs (ClubName, MeetDay, MeetTime) values (@ClubName, @MeetDay, @MeetTime)";

            //Create our SQL parameters array to pass through the SQL query
            SqlParameter[] sqlparams = new SqlParameter[3];
            sqlparams[0] = new SqlParameter("@ClubName", ClubName);
            sqlparams[1] = new SqlParameter("@MeetDay", MeetDay);
            sqlparams[2] = new SqlParameter("@MeetTime", MeetTime);

            //write a debug line to make sure that we are pulling the correct data
            Debug.WriteLine("Trying to create a new book club called " + ClubName + " which meets on " + MeetDay + " at " + MeetTime);

            //run a Database command to insert the values into the database and add the book
            db.Database.ExecuteSqlCommand(query, sqlparams);

            //Once successful, go back to the list page
            return RedirectToAction("List");

        }

        //SET: Update values for book club record
        //Get existing values for the update form from database
        public ActionResult Update(int id)
        {
            //Pull the data from the database about specific book club
            BookClub selectedclub = db.BookClubs.SqlQuery("Select * from BookClubs where ClubID = @id", new SqlParameter("@id", id)).FirstOrDefault();
            //get Book data for list of books
            List<Book> Books = db.Books.SqlQuery("select * from Books").ToList();

            //connect to ViewModel
            UpdateClub UpdateBookViewModel = new UpdateClub();
            UpdateBookViewModel.bookclub = selectedclub;
            UpdateBookViewModel.books = Books;

            return View(UpdateBookViewModel);

        }
        //Add the updated information once the user clicks Update button
        [HttpPost]
        public ActionResult Update(int id, string ClubName, string MeetDay, string MeetTime, int BookID)
        {
            //What happens when we submit the form? Update the database record
            string query = "update BookClubs set ClubName = @ClubName, MeetDay = @MeetDay, MeetTime = @MeetTime where ClubID = @id";

            //Set up our Sqlparameter array
            SqlParameter[] sqlparams = new SqlParameter[4];
            sqlparams[0] = new SqlParameter("@id", id);
            sqlparams[1] = new SqlParameter("@ClubName", ClubName);
            sqlparams[2] = new SqlParameter("@MeetDay", MeetDay);
            sqlparams[3] = new SqlParameter("@MeetTime", MeetTime);

            db.Database.ExecuteSqlCommand(query, sqlparams);


            //add book to club
            string bookquery = "insert into BookxClubs (BookClubID, BookID) values (@id, @BookID)";
            SqlParameter[] book_sqlparams = new SqlParameter[2];
            book_sqlparams[0] = new SqlParameter("@id", id);
            book_sqlparams[1] = new SqlParameter("@BookID", BookID);

            //execute the query
            db.Database.ExecuteSqlCommand(bookquery, book_sqlparams);

            //Test to see what data we're pushing through and if it's working
            Debug.WriteLine("Trying to update book club called " + ClubName + " which meets on " + MeetDay + " at " + MeetTime);

            //Send user back to the List of all books after submission
            return Redirect("/BookClub/Show/" + id);

        }

        //SET: delete a book club from the database
        public ActionResult DeleteConfirm(int id)
        {
            string query = "select * from BookClubs where ClubID = @id";
            SqlParameter param = new SqlParameter("@id", id);
            BookClub selectedclub = db.BookClubs.SqlQuery(query, param).FirstOrDefault();

            return View(selectedclub);
        }
        //Complete deletion after "Confirm" button selected in confirm form
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string query = "delete from BookClubs where ClubID = @id";
            SqlParameter param = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, param);

            return RedirectToAction("List");
        }



    }
}
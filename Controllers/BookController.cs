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


namespace BookClubMVC.Controllers
{
    public class BookController : Controller
    {

        //Call new database connection for all logic
        private BookClubContext db = new BookClubContext();

        //GET all book information from the database in a list form

        public ActionResult List(string booksearch)
         {
             //create a debug line to see if we can access the search query user is trying to use
             Debug.WriteLine("The search term is " + booksearch);

             //declare the SQL query in a variable to use with search term
             string query = "Select * from Books";

             //take search term and add it to our query in an if statement

             if (booksearch != "")
             {
                 //concat our search query with sql query
                 query = query + " where Title like '%" + booksearch + "%'"; //****FIX THIS ******

                 //Call our debug line to show what our SQL query was and confirm accuracy
                 Debug.WriteLine("Our query is " + query);
             }

             //Display results of search in a list
             List<Book> books = db.Books.SqlQuery(query).ToList();
             return View(books);
         }
        /* public ActionResult List()
        {
            List<Book> books = db.Books.SqlQuery("Select * from Books").ToList();
            Debug.WriteLine("SQL query successful");
            return View(books);

        } */

        public ActionResult New()
        {
            return View();
        }

        public ActionResult Show(int? id)
        {
            //make sure that there is an ID selected to navigate to Book/Show/[id]
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Make our database connection with for Book model
            // Create a paramaterized SQL query to bring up information for one book where our BookIDs match.
            Book Book = db.Books.SqlQuery("Select * from books where BookID=@BookId", new SqlParameter("@BookId", id)).FirstOrDefault();

            // Create an error message for if there is no book for current ID (an invalid ID)
            if (Book == null)
            {
                return HttpNotFound();
            }

            //We need to pull review information for each individual book
            string query = "select * from Reviews inner join Members on Reviews.MemberID = Members.MemberID where Reviews.BookID = @id";
            //create SQL Parameter to prevent SQL injections
            SqlParameter param = new SqlParameter("@id", id);
            List<Review> Review = db.Reviews.SqlQuery(query, param).ToList();

            Models.ViewModels.ShowBook viewmodel = new Models.ViewModels.ShowBook();
            viewmodel.book = Book;
            viewmodel.reviews = Review;
            //viewmodel.reviewmember = ReviewMember; 

            return View(viewmodel);
        }

        [HttpPost]
        public ActionResult New(string Title, string Author, string Description)
        {
            // decalre what our query is going to be by pulling the name for each form input on the New.cshtml page
            string query = "Insert into Books (Title, Author, Description) values (@Title, @Author, @Description)";

            //Create our SQL parameters array to pass through the SQL query
            SqlParameter[] sqlparams = new SqlParameter[3];
            sqlparams[0] = new SqlParameter("@Title", Title);
            sqlparams[1] = new SqlParameter("@Author", Author);
            sqlparams[2] = new SqlParameter("@Description", Description);

            //write a debug line to make sure that we are pulling the correct data
            Debug.WriteLine("Trying to create a new book titled" + Title + " by " + Author + " about " + Description);

            //run a Database command to insert the values into the database and add the book
            db.Database.ExecuteSqlCommand(query, sqlparams);

            //Once successful, go back to the list page
            return RedirectToAction("List");

        }

        public ActionResult Update(int id)
        {
            //Pull the data from the database about specific book 
            Book selectedbook = db.Books.SqlQuery("Select * from Books where BookID = @id", new SqlParameter ("@id", id)).FirstOrDefault();

            return View(selectedbook);

        }
        [HttpPost]
        public ActionResult Update(int id, string Title, string Author, string Description)
        {
            //What happens when we submit the form? Update the database record

            string query = "update Books set Title = @Title, Author = @Author, Description = @Description where BookID = @id";

            //Set up our Sqlparameter array
            SqlParameter[] sqlparams = new SqlParameter[4];
            sqlparams[0] = new SqlParameter("@id", id);
            sqlparams[1] = new SqlParameter("@Title", Title);
            sqlparams[2] = new SqlParameter("@Author", Author);
            sqlparams[3] = new SqlParameter("@Description", Description);

            //execute the query
            db.Database.ExecuteSqlCommand(query, sqlparams);

            //Test to see what data we're pushing through and if it's working
            Debug.WriteLine("Trying to update book " + id + " titled " + Title + " by " + Author + " about " + Description);

            //Send user back to the List of all books after submission
            return RedirectToAction("List");

        }

        public ActionResult DeleteConfirm(int id)
        {
            //get one single book to delete
            string query = "select * from Books where BookID = @id";
            SqlParameter param = new SqlParameter("@id", id);
            Book selectbook = db.Books.SqlQuery(query, param).FirstOrDefault();

            return View(selectbook);
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            //Complete deletion after "Confirm" button selected in confirm form
            string query = "delete from Books where BookID = @id";
            SqlParameter param = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, param);

            return RedirectToAction("List");
        }

    }
}
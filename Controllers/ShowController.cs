using BookMyTicket.Models;
using BookMyTicket.ViewModel;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace BookMyTicket.Controllers
{
    //2:40

    [Authorize]
    public class ShowController : Controller
    {
        AdityaEntities4 db = new AdityaEntities4();

        static int globleleftseat;

        static int globlelBookingId;

        static string globalCustomername;

        static int fetchingId;

        static TimeSpan globalShowTime;

        // access to all
        // GET: Show
        public ActionResult Index()
        {
            if (User.IsInRole("AdminProfile"))
            {
                return View("IndexForAdmin", db.Shows.ToList());
            }
            else
            {
                return View("IndexForUser", db.Shows.ToList());
            }


        }

        // first create screenlist

        [Authorize(Roles = "AdminProfile")]
        public ActionResult ScreenList()
        {
            return View(db.Screens.ToList());
        }


        [Authorize(Roles = "AdminProfile")]
        public ActionResult SelectCity()
        {

            TheatreViewModel obj = new TheatreViewModel()
            {
                cityList = db.Cities.ToList()
            };

            return View(obj);
        }




        [Authorize(Roles = "AdminProfile")]
        public ActionResult NewScreen(TheatreViewModel tvm)
        {


            ScreenViewModel obj = new ScreenViewModel()
            {
                theatreList = db.Theatres.Where(temp => temp.CityId == tvm.theatre.CityId).ToList()
            };

            return View(obj);
        }










        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminProfile")]
        public ActionResult NewScreen1(Screen screen)
        {
            if (ModelState.IsValid)
            {

                var noOfScreenInTheatre = db.Theatres.Single(temp => temp.TheatreId == screen.TheatreId);

                var value = noOfScreenInTheatre.NoOfScreen;

                var countInScreendb = db.Screens.Count(temp => temp.TheatreId == screen.TheatreId);

                if (countInScreendb >= value)
                {
                    ViewBag.NoOfScreenExhausted = "You cannot add more screens to this theatre";

                    ScreenViewModel obj = new ScreenViewModel()
                    {
                        theatreList = db.Theatres.ToList()
                    };



                    return View("NewScreen", obj);

                }
                else
                {
                    db.Screens.Add(screen);
                    db.SaveChanges();

                    return View("ScreenList", db.Screens.ToList());
                }
            }
            else
            {
                ScreenViewModel obj = new ScreenViewModel()
                {
                    theatreList = db.Theatres.ToList(),
                    screen = screen
                };

                return View("NewScreen", obj);
            }

        }

        [Authorize(Roles = "AdminProfile")]
        public ActionResult ShowForm(int id)
        {
            fetchingId = id;

            Show oShow = new Show();
            oShow.ScreenId = id;

            ShowViewModel obj = new ShowViewModel();
            var today = DateTime.Now;
            var onemonthbefore = today.AddMonths(-1);
            obj.movieList = db.Movies.Where(temp => temp.DateRelease >= onemonthbefore).OrderByDescending(temp => temp.DateRelease).ToList();



            obj.show = oShow;
            obj.screen = db.Screens.SingleOrDefault(temp => temp.ScreenId == id);


            return View(obj);
        }


        // public ActionResult CreateBookingForm(int id)

        // access to all
        public ActionResult CreateBookingForm(int Show_id, int Screen_id)
        {

            Booking BookingObj = new Booking()
            {
                ShowId = Show_id

            };

            // Session["UserEmail"]     //use session here


            BookingViewModel obj = new BookingViewModel()
            {
                show = db.Shows.SingleOrDefault(temp => temp.ShowId == Show_id),
                booking = BookingObj,
                screen = db.Screens.SingleOrDefault(temp => temp.ScreenId == Screen_id)
            };

            globalShowTime = obj.show.Time;

            return View(obj);
        }


        // access to all
        [HttpGet]
        public JsonResult BookingAvailablity(System.DateTime DateOfBooking, int ShowId/*, int NoOfSeats*/)
        {


            using (var context = new AdityaEntities4())

            {


                var objData = from f in context.Bookings
                              where f.DateOfBooking == DateOfBooking
                              && f.ShowId == ShowId
                              group f by new { f.DateOfBooking, f.ShowId } into t
                              select new
                              {

                                  TotalNoOfSeats = t.Sum(s => s.NoOfSeats)
                              };


                int numberOfSeats = 0;

                foreach (var a in objData)
                {
                    numberOfSeats = a.TotalNoOfSeats;
                }
                //  no of seat 157

                var show = db.Shows.SingleOrDefault(temp => temp.ShowId == ShowId);

                var var1 = show.ScreenId;


                var screenindb = db.Screens.SingleOrDefault(temp => temp.ScreenId == var1);


                int leftseats = screenindb.ScreenCapacity - numberOfSeats;

                var value = screenindb.ScreenCapacity;

                if (numberOfSeats >= value)
                {
                    leftseats = 0;
                }


                globleleftseat = leftseats;

                /*     if (NoOfSeats > leftseats)
                     {


                         return Json(leftseats,JsonRequestBehavior.AllowGet);    // return leftseats;

                     }*/

                return Json(leftseats, JsonRequestBehavior.AllowGet);    //return leftseats;
            }

        }




        // access to all
        [ValidateAntiForgeryToken]
        public ActionResult BookingCalculation(BookingViewModel bvm)
        {
            var vardate = bvm.booking.DateOfBooking;

            var today = DateTime.Now;

            if (vardate.Date < today.Date)
            {
                return Content("Please provied valid date and try again");
            }

            //   globleleftseat

            if (globleleftseat < bvm.booking.NoOfSeats)
            {
                return Content("Error occured please try again");
            }

            //    if (ModelState.IsValid)
            //    {


            bvm.booking.TotalAmount = bvm.booking.NoOfSeats * bvm.show.Rate;


            ViewBag.Amount = bvm.booking.TotalAmount;

            return View("CreateBookingForm", bvm);
            //       }


            //return View("CreateBookingForm", bvm);
        }





        // this method will update the database
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminProfile")]
        [HttpPost]
        public ActionResult CreateNewShow(ShowViewModel obj)
        {


            if (ModelState.IsValid)
            {

                if (obj.show.ShowId == 0)
                {
                    db.Shows.Add(obj.show);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Show");
                }

            }





            Show oShow = new Show();
            oShow.ScreenId = fetchingId;

            ShowViewModel obj1 = new ShowViewModel();
            var today = DateTime.Now;
            var onemonthbefore = today.AddMonths(-1);
            obj1.movieList = db.Movies.Where(temp => temp.DateRelease >= onemonthbefore).OrderByDescending(temp => temp.DateRelease).ToList();



            obj1.show = oShow;
            obj1.screen = db.Screens.SingleOrDefault(temp => temp.ScreenId == fetchingId);




            return View("ShowForm", obj1);


        }

        [Authorize(Roles = "AdminProfile")]
        public ActionResult EditShowForm(int id)
        {
            var today = DateTime.Now;

            var onemonthbefore = today.AddMonths(-1);



            ShowViewModel obj = new ShowViewModel()
            {
                movieList = db.Movies.Where(temp => temp.DateRelease >= onemonthbefore).ToList(),
                screenlist = db.Screens.ToList(),
                show = db.Shows.SingleOrDefault(temp => temp.ShowId == id)
            };
            return View(obj);
        }


        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminProfile")]
        [HttpPost]
        public ActionResult EditShowForm1(Show show)
        {
            if (ModelState.IsValid)
            {
                var showindb = db.Shows.SingleOrDefault(temp => temp.ShowId == show.ShowId);

                if (showindb == null)
                {
                    return HttpNotFound();
                }
                else
                {
                    showindb.MovieId = show.MovieId;
                    showindb.ScreenId = show.ScreenId;
                    showindb.Rate = show.Rate;
                    showindb.Time = show.Time;

                    db.SaveChanges();
                }
                return RedirectToAction("Index", "Show");

            }
            else
            {

                var today = DateTime.Now;

                var onemonthbefore = today.AddMonths(-1);

                ShowViewModel obj = new ShowViewModel()
                {
                    movieList = db.Movies.OrderByDescending(temp => temp.DateRelease >= onemonthbefore).ToList(),
                    show = db.Shows.SingleOrDefault(temp => temp.ShowId == show.ShowId)

                };

                return View("EditShowForm", obj);

            }


        }





        /*   [HttpDelete]
           public ActionResult DeleteShow(int id)
           {
               var showindb = db.Shows.SingleOrDefault(temp => temp.ShowId == id);

               if (showindb == null)
               {
                   return HttpNotFound();
               }
               else
               {
                   List<Booking> deletebookinglist = db.Bookings.Where(temp => temp.ShowId == id).ToList();

                   foreach (var singlebooking in deletebookinglist)
                   {
                       db.Bookings.Remove(singlebooking);
                       db.SaveChanges();
                   }



                   db.Shows.Remove(showindb);
                   db.SaveChanges();


                   return RedirectToAction("Index", "Show");
               }

           }

       */


        //      this is boolean version
        [Authorize(Roles = "AdminProfile")]
        [HttpGet]
        public JsonResult DeleteShow(int id)
        {
            var showindb = db.Shows.SingleOrDefault(temp => temp.ShowId == id);

            if (showindb == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);//false;


            }
            else
            {
                List<Booking> deletebookinglist = db.Bookings.Where(temp => temp.ShowId == id).ToList();

                foreach (var singlebooking in deletebookinglist)
                {
                    db.Bookings.Remove(singlebooking);
                    db.SaveChanges();
                }



                db.Shows.Remove(showindb);
                db.SaveChanges();



                return Json(true, JsonRequestBehavior.AllowGet);//true;



            }

        }







        //         [HttpDelete]
        //public JsonResult DeleteShow(int id)
        //{
        //    var showindb = db.Shows.SingleOrDefault(temp => temp.ShowId == id);

        //    if (showindb == null)
        //    {
        //                return Json(false, JsonRequestBehavior.AllowGet); //HttpNotFound();
        //    }
        //    else
        //    {
        //        List<Booking> deletebookinglist = db.Bookings.Where(temp => temp.ShowId == id).ToList();

        //        foreach (var singlebooking in deletebookinglist)
        //        {
        //            db.Bookings.Remove(singlebooking);
        //            db.SaveChanges();
        //        }



        //        db.Shows.Remove(showindb);
        //        db.SaveChanges();


        //                return Json(true, JsonRequestBehavior.AllowGet);   //RedirectToAction("Index", "Show");
        //    }

        //}



        // access to all
        public ActionResult Payment(Booking booking)
        {


            BookingViewModel obj = new BookingViewModel()
            {
                booking = booking,
                paymentlist = db.Payments.ToList(),
                show = db.Shows.SingleOrDefault(temp => temp.ShowId == booking.ShowId)
            };

            return View(obj);
        }

        // access to all
        public ActionResult BookingConfirm(Booking booking)
        {
            if (booking.BookingId == 0)
            {

                var noofcount = booking.NoOfSeats;


                if (noofcount > globleleftseat)
                {
                    return Content("error occured please try again");
                }

                if (noofcount == globleleftseat || noofcount < globleleftseat)
                {

                    booking.TimeOfBooking = DateTime.Now;

                    var UserIdinDb = User.Identity.GetUserId();

                    var emailindb = db.AspNetUsers.Single(temp => temp.Id == UserIdinDb);

                    var nameindb = db.Customers.Single(temp => temp.Id == UserIdinDb);

                    globalCustomername = nameindb.CustomerName;

                    booking.Email = emailindb.Email;


                    db.Bookings.Add(booking);
                    db.SaveChanges();



                    var bookingid = db.Bookings.Where(temp => temp.Email == booking.Email).OrderByDescending(temp => temp.BookingId).First();


                    globlelBookingId = bookingid.BookingId;

                    // just testing here

                    var var1 = db.Bookings.Single(temp => temp.BookingId == globlelBookingId);

                    var var2 = db.Shows.Single(temp => temp.ShowId == var1.ShowId);
                    var moviename = db.Movies.Single(temp => temp.MovieId == var2.MovieId);
                    Session["moviename"] = moviename.MovieName;




                    sendEmailMethod(booking);

                    TempData["CheckMailMessage"] = "Booking confirmed. please check you email";



                    return RedirectToAction("Index", "Show");
                }
                else
                {
                    return Content("error occured please try again");
                }

            }
            else
            {


                return Content("error occured please try again");
            }

        }

        // access to all
        public void sendEmailMethod(Booking b)
        {

            Email obj = new Email
            {
                Body = "Hi " + globalCustomername + ",</br></br>Thank you for using BookMyTicket. Your booking no. is " + globlelBookingId + ".</br> Booking date " + b.DateOfBooking.ToString("yyyy-MM-dd") + "  for " + Session["moviename"] + " at " + globalShowTime + " and number of seats are " + b.NoOfSeats + ".",
                From = "admin@BookMyTicket.com",
                Subject = "Booking Confirmation mail",
                To = b.Email
            };
            Session["moviename"] = null;


            if (ModelState.IsValid)
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(obj.To);
                mail.From = new MailAddress(obj.From);
                mail.Subject = obj.Subject;
                string Body = obj.Body;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "	smtp.mailtrap.io";
                smtp.Port = 25;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential("0d641e048c0b8d", "8a0f3c27bd6ab2"); // Enter seders User name and password  
                smtp.EnableSsl = true;
                smtp.Send(mail);
                //  return View("Index", obj);
            }

        }


        // access to all
        public string GetCity(int Theatreid)
        {

            var theatreindb = db.Theatres.Single(temp => temp.TheatreId == Theatreid);

            var city = theatreindb.City.CityName;

            return (city);
        }

    }
}









using BookMyTicket.ViewModel;
using Newtonsoft.Json;
using PagedList;
using PagedList.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace BookMyTicket.Controllers
{
    [Authorize]
    public class MovieController : Controller
    {
        AdityaEntities4 db = new AdityaEntities4();



        // access to all
        public ActionResult Test2(int? page) // used  by user
        {
            return View(db.Movies.ToList().OrderByDescending(temp => temp.DateRelease).ToPagedList(page ?? 1, 8));
        }

        //public ActionResult NowShowing(int? page) // used  by user
        //{
        //    var today = DateTime.Now;

        //    var  onemonthbefore = today.AddMonths(-1);

        //    return View(db.Movies.Where(temp=>temp.DateRelease >= onemonthbefore).ToList().OrderByDescending(temp => temp.DateRelease).ToPagedList(page ?? 1, 8));
        //}

        //access to all
        public ActionResult cardButtonPress(int movieid)
        {
            var testvariable = db.Shows.Where(temp => temp.Movie.MovieId == movieid).ToList();

            return View(testvariable);
        }


        // access to all
        public ActionResult MovieDetails(int movieid)
        {
            var singleMovieInDb = db.Movies.Single(temp => temp.MovieId == movieid);

            return View(singleMovieInDb);
        }




        [Authorize(Roles = "AdminProfile")]
        public string CheckDetails(string param1, string param2)
        {
            var currentdate = DateTime.Now;

            var onemonthback = currentdate.AddMonths(-1);


            var movieindb = (from k in db.Movies.Where(temp => temp.DateRelease >= onemonthback)
                             select new
                             {
                                 MovieId = k.MovieId,
                                 MovieName = k.MovieName,
                                 DateRelease = k.DateRelease,
                                 DateEnd = k.DateEnd,
                                 LanguageId = k.LanguageId,
                                 GenreId = k.GenreId,
                                 Description = k.Description,
                                 CreatedDate = k.CreatedDate,
                                 ModifiedDate = k.ModifiedDate,
                                 CreatedBy = k.CreatedBy,
                                 ModifiedBy = k.ModifiedBy,
                                 MoviePic = k.MoviePic,
                                 MovieWallpaper = k.MovieWallpaper
                             }).ToList();

           

            List<Movie> movielist = new List<Movie>();

            var chk1 = new Movie
            {
                MovieName = "Movie1 " + param1,
                Description = param2 + " Description1"
            };
            var chk2 = new Movie
            {
                MovieName = "Movie2 " + param1,
                Description = param2 + " Description2"
            };
            var chk3 = new Movie
            {
                MovieName = "Movie3 " + param1,
                Description = param2 + " Description3"
            };

            movielist.Add(chk1);
            movielist.Add(chk2);
            movielist.Add(chk3);



            return JsonConvert.SerializeObject(movieindb);
        }



        // access to all
        [HttpGet]
        public JsonResult getMovies() // this is used for autocomplete
        {
            return Json(db.Movies.Select(temp => temp.MovieName).ToList(), JsonRequestBehavior.AllowGet);
        }


        // access to all

        [ValidateAntiForgeryToken]
        public ActionResult getSingleMovie(string search) // this is used for singlemoviesearch
        {
            try
            {
                // var singlemovieindb = db.Movies.Single(temp => temp.MovieName == search);

                //  var var1 = db.Movies.Single(temp => temp.MovieName == search).ToPagedList(1, 1);

                var singlemovieindb = db.Movies.Where(temp => temp.MovieName == search).ToList().ToPagedList(1, 1);

                if (singlemovieindb.Count == 0)
                {
                    throw new Exception();
                }

                if (singlemovieindb != null)
                {
                    // IEnumerable<Movie> singlemovie = new Movie[] { singlemovieindb };

                    // var var1 = singlemovie;
                    // PagedList.IPagedList`1[BookMyTicket.Movie]'

                    //     IPagedList<Movie> = new Movie[] { singlemovieindb };



                    ViewBag.message = "successful";
                    return View("Test2", singlemovieindb);
                }
            }
            catch/*(Exception e)*/
            {


                ViewBag.message = "Movie not found please provide correct name";
                return View("Test2", db.Movies.ToList().OrderByDescending(temp => temp.DateRelease).ToPagedList(1, 8));
            }


            return null;

        }


        // access to all
        // GET: Movie
        public ActionResult Index(int? page)
        {
            if (User.IsInRole("AdminProfile"))
            {
                return View("AdminMovieList", db.Movies.ToList());
            }
            else
            {

                var today = DateTime.Now;

                var onemonthbefore = today.AddMonths(-1);

                return View("NowShowing", db.Movies.Where(temp => temp.DateRelease >= onemonthbefore).ToList().OrderByDescending(temp => temp.DateRelease).ToPagedList(page ?? 1, 8));

            }

        }

        [Authorize(Roles = "AdminProfile")]
        public ActionResult MovieForm()
        {
            MovieViewModel obj = new MovieViewModel()
            {
                genrelist = db.Genres.ToList(),
                languagelist = db.Languages.ToList()
            };
            return View(obj);
        }



        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminProfile")]
        [HttpPost]
        public ActionResult CreateMovie(MovieViewModel obj)
        {

            if (obj.movie.MovieId == 0)
            {
                db.Movies.Add(obj.movie);
                db.SaveChanges();
            }

            return RedirectToAction("Index", "Movie");
        }


        //edit





        [Authorize(Roles = "AdminProfile")]
        public ActionResult EditMovieForm(int id)
        {
            MovieViewModel obj = new MovieViewModel()
            {
                genrelist = db.Genres.ToList(),
                languagelist = db.Languages.ToList(),
                movie = db.Movies.SingleOrDefault(temp => temp.MovieId == id)
            };
            return View(obj);
        }



        [ValidateAntiForgeryToken]
        [Authorize(Roles = "AdminProfile")]
        public ActionResult EditMovie(MovieViewModel mvm)
        {
            if (!ModelState.IsValid)
            {
                return Content("Some Error occured");
            }

            else
            {
                var movieindb = db.Movies.SingleOrDefault(temp => temp.MovieId == mvm.movie.MovieId);

                if (movieindb == null)
                {
                    return Content("Movie Does not Exist");
                }
                else
                {
                    movieindb.MovieName = mvm.movie.MovieName;
                    movieindb.DateRelease = mvm.movie.DateRelease;
                    movieindb.DateEnd = mvm.movie.DateEnd;
                    movieindb.LanguageId = mvm.movie.LanguageId;
                    movieindb.GenreId = mvm.movie.GenreId;
                    movieindb.Description = mvm.movie.Description;
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index", "Movie");
        }



        //delete



        //public ActionResult DeleteMovie(int id)
        //{       

        //        var movieindb = db.Movies.SingleOrDefault(temp => temp.MovieId == id);

        //        if (movieindb == null)
        //        {
        //            return Content("Movie Does not Exist");
        //        }
        //        else
        //        {
        //            db.Movies.Remove(movieindb);
        //            db.SaveChanges();
        //        }


        //    return RedirectToAction("Index", "Movie");
        //}

        [Authorize(Roles = "AdminProfile")]
        [HttpGet]
        public JsonResult DeleteMovie(int id)
        {

            var movieindb = db.Movies.SingleOrDefault(temp => temp.MovieId == id);

            if (movieindb == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);   //Content("Movie Does not Exist");
            }
            else
            {
                db.Movies.Remove(movieindb);
                db.SaveChanges();
            }


            return Json(true, JsonRequestBehavior.AllowGet);  //RedirectToAction("Index", "Movie");
        }

    }
}
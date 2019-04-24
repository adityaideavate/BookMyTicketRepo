using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace BookMyTicket.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        AdityaEntities4 db = new AdityaEntities4();

        public ActionResult Index()
        {
             var today = DateTime.Now;

            var  onemonthbefore = today.AddMonths(-1);

            return View(db.Movies.ToList().OrderByDescending(temp => temp.DateRelease));
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
using BookMyTicket.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookMyTicket.Controllers
{
    public class CustomerController : Controller
    {
        AdityaEntities4 db = new AdityaEntities4();
        // GET: Customer
        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }


        public ActionResult CustomerForm(string id)
        {
            var aspnetuserindb = db.AspNetUsers.SingleOrDefault(temp=>temp.Id == id);

            Customer anu = new Customer() {
               Id=id
            };

            CustomerViewForm cvf = new CustomerViewForm()
            {
                Customer=anu
            };


            return View(cvf);
        }

        public ActionResult CreateCustomer(Customer Customer)
        {
            if (Customer.CustomerId == 0)
            {
                db.Customers.Add(Customer);
                db.SaveChanges();
                    
            }


            return RedirectToAction("Index","Show");
        }




    }
}
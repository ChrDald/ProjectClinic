using Microsoft.AspNet.Identity;
using ProjectClinic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectClinic.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
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

        [Authorize(Roles = "Patient")]
        public ActionResult Patients()
        {            
            if (User.IsInRole("Patient"))
            {
                string currentUserId = User.Identity.GetUserId();
                //ApplicationUser currentUser = context.Users.FirstOrDefault(x => x.Id == currentUserId);

                Patient patient = (from p in context.Patient where p.UserId == currentUserId select p).SingleOrDefault();
                return View(patient);
            }
            else
            {
                return HttpNotFound();
            }
            
        }

        [Authorize(Roles ="Doctor")]
        public ActionResult Doctors()
        {
            return View();
        }

    }
}
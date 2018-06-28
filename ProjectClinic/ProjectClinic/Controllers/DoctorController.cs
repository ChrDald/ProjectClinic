using Microsoft.AspNet.Identity;
using ProjectClinic.Models;
using ProjectClinic.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectClinic.Controllers
{
    [Authorize(Roles = "Doctor")]
    public class DoctorController : Controller
    {

        ApplicationDbContext context = new ApplicationDbContext();
        
        // GET: Doctor
        public ActionResult Index()
        {
            return View();
            
        }

        public ActionResult ConsultProfiles()
        {
            List<Patient> patients = (from p in context.Patient
                               join c in context.Condition
                               on p.Id equals c.PatientId
                               select p).Distinct().ToList();

            return View(patients);
        }

        public PartialViewResult ViewPatientFile(int id)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("PatientFilePartial", GetConditionsById(id));
            }
            else
            {
                return PartialView();
            }
        }

        public ActionResult ManageAppointments()
        {
            List<ManageAppointmentsDoctorViewModel> vml = new List<ManageAppointmentsDoctorViewModel>();

            Doctor doctor = GetDoctorInContext();

            doctor.Appointments = GetDoctorAppointments(doctor);

            foreach (var item in doctor.Appointments)
            {
                ManageAppointmentsDoctorViewModel vm = new ManageAppointmentsDoctorViewModel
                {
                    Appointment = item,
                    PatientName =
                    (
                    (from p in context.Patient where p.Id == item.PatientId select p.FirstName).SingleOrDefault()
                    + " " +
                    (from p in context.Patient where p.Id == item.PatientId select p.LastName).SingleOrDefault()
                    )
                    // probably not an efficient way of doing this
                };
                vml.Add(vm);
            }
            return View(vml);
        }

        public PartialViewResult EditAppointment(int id)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("EditAppointmentPartial", GetAppointmentById(id));
            }
            else
            {
                return PartialView();
            }

        }

        public PartialViewResult CancelAppointment(int id)
        {
            if (Request.IsAjaxRequest())
            {
                return PartialView("DeleteAppointmentPartial", GetAppointmentById(id));
            }
            else
            {
                return PartialView();
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteAppointment(Appointment apt)
        {
            var aptInDb = (from a in context.Appointment where a.Id == apt.Id select a).SingleOrDefault();

            context.Appointment.Remove(aptInDb);
            context.SaveChanges();
            return RedirectToAction("ManageAppointments");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveAppointment(Appointment apt)
        {
            if (!ModelState.IsValid)
            {
                return View("ManageAppointments");

            }

            else
            {
                var aptInDb = (from a in context.Appointment where a.Id == apt.Id select a).SingleOrDefault();

                aptInDb.DateTime = apt.DateTime;

                context.SaveChanges();

                return RedirectToAction("ManageAppointments");
            }


        }

        public ActionResult DoctorProfile()
        {
            return View(GetDoctorInContext());
        }

        public ActionResult EditProfile()
        {
            return View(GetDoctorInContext());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveProfile(Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                //The form is not valid --> return same form to the user
                return View("EditProfile", (GetDoctorInContext()));

            }
            var DoctorInDb = GetDoctorInContext();

            DoctorInDb.FirstName = doctor.FirstName;
            DoctorInDb.LastName = doctor.LastName;
            DoctorInDb.Phone = doctor.Phone;
            DoctorInDb.Email = doctor.Email;

            context.SaveChanges();

            return RedirectToAction("Doctors", "Home");
        }

        // context methods
        public List<Condition> GetConditionsById(int id)
        {
            return (from c in context.Condition where c.PatientId == id select c).ToList();
        }

        public Appointment GetAppointmentById(int id)
        {
            return (from a in context.Appointment where a.Id == id select a).SingleOrDefault();
        }

        private Doctor GetDoctorInContext()
        {
            if (User.IsInRole("Doctor"))
            {
                string currentUserId = User.Identity.GetUserId();

                return (from d in context.Doctor where d.UserId == currentUserId select d).SingleOrDefault();

            }
            else
            {
                return null;
            }
        }

        private List<Appointment> GetDoctorAppointments(Doctor doctor)
        {
            return (from a in context.Appointment
                    join d in context.Doctor
                    on a.DoctorId equals d.Id
                    select a).ToList();
        }
    } 
}
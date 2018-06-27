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
    [Authorize(Roles = "Patient")]
    public class PatientController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
     

        // GET: Patient
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PatientProfile()
        {
            return View(GetPatientInContext());
        }

        public ActionResult EditProfile()
        {
            return View(GetPatientInContext());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveProfile(Patient patient)
        {
            if (!ModelState.IsValid)
            {
                //The form is not valid --> return same form to the user
                return View("EditProfile", (GetPatientInContext()));

            }
            var patientInDb = GetPatientInContext();

            patientInDb.FirstName = patient.FirstName;
            patientInDb.LastName = patient.LastName;
            patientInDb.Phone = patient.Phone;
            patientInDb.Email = patient.Email;
            patientInDb.DateOfBirth = patient.DateOfBirth;
            patientInDb.MedicalCardNo = patient.MedicalCardNo;

            context.SaveChanges();

            return RedirectToAction("Patients", "Home");
        }

        public ActionResult ManageAppointments()
        {
            Patient patient = GetPatientInContext();
            patient.Appointments = GetPatientAppointments(patient);

            return View(patient);
        }

        // returns the patient model associated with the logged in user account
        private Patient GetPatientInContext()
        {
            if (User.IsInRole("Patient"))
            {
                string currentUserId = User.Identity.GetUserId();

                return (from p in context.Patient where p.UserId == currentUserId select p).SingleOrDefault();
               
            }
            else
            {
                return null;
            }
        }
        private List<Appointment> GetPatientAppointments(Patient patient)
        {
            return (from a in context.Appointment
                        join p in context.Patient
                        on a.PatientId equals p.Id
                        select a).ToList();
        }
    }
}
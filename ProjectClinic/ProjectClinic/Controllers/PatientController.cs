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
            List<ManageAppointmentsViewModel> vml = new List<ManageAppointmentsViewModel>();
            

            Patient patient = GetPatientInContext();
            patient.Appointments = GetPatientAppointments(patient);

            foreach (var item in patient.Appointments)
            {
                ManageAppointmentsViewModel vm = new ManageAppointmentsViewModel
                {
                    Appointment = item,
                    DoctorName = (from d in context.Doctor where d.Id == item.DoctorId select d.FirstName).SingleOrDefault()
                };
                vml.Add(vm);
            }
            // because i would like to display the doctor name not just the id
            return View(vml);
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

        public Appointment GetAppointmentById(int id)
        {
            return (from a in context.Appointment where a.Id == id select a).SingleOrDefault();
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

            if (apt.Id == 0)
            {
                // new appointment
                // for now just assign a random doctor
                int[] doctors = (from d in context.Doctor select d.Id).ToArray();

                //randoms apprently not working properly
                Random random = new Random();
                int randomId = random.Next(doctors.First(), doctors.Last());
                apt.DoctorId = randomId;

                apt.PatientId = GetPatientInContext().Id;

                context.Appointment.Add(apt);
                context.SaveChanges();

                return RedirectToAction("ManageAppointments");
            }
            else
            {
                var aptInDb = (from a in context.Appointment where a.Id == apt.Id select a).SingleOrDefault();

                aptInDb.DateTime = apt.DateTime;

                context.SaveChanges();

                return RedirectToAction("ManageAppointments");
            }
            

        }

        public ActionResult BookAppointment()
        {
            
            return View();
        }

        [HttpPost]
        public JsonResult CheckAppointmentAvail(string aptTime)
        {
            bool isValid = true;

            DateTime selectedDate = DateTime.Parse(aptTime);

            List<DateTime> takenTimes = (from a in context.Appointment select a.DateTime).ToList();

            foreach (var item in takenTimes)
            {
                if (item.CompareTo(selectedDate) == 0)
                {
                    isValid = false;
                    return Json(isValid);
                }
            }
            return Json(isValid);
        }

    }
}
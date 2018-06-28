using ProjectClinic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectClinic.ViewModels
{
    public class ManageAppointmentsViewModel
    {
        public Appointment Appointment { get; set; }
        public string DoctorName { get; set; }
    }
}
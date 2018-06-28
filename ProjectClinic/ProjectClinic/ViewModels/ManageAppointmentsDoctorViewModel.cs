using ProjectClinic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectClinic.ViewModels
{
    public class ManageAppointmentsDoctorViewModel
    {
        public Appointment Appointment { get; set; }
        public string PatientName { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectClinic.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateTime DateTime { get; set; }
    } 
}
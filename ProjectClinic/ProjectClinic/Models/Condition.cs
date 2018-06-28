using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectClinic.Models
{
    public class Condition
    {
        public int Id { get; set; }

        [Required]
        public int PatientId { get; set; }

        public string Description { get; set; }

        /*
        public List<string> Allergies { get; set; }

        public List<string> Medication { get; set; }
        */
    }
}
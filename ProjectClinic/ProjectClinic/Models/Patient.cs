using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ProjectClinic.Models
{
    public class Patient
    {
        public int Id { get; set; }

        // medical conditions
        public List<Condition> Conditions { get; set; }
        
        // to link with user account
        public string UserId { get; set; }


        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [StringLength(20)]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }

        [StringLength(50)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [StringLength(16)]
        [Display(Name = "Medical Card Number")]
        public string MedicalCardNo { get; set; }

        [Required]
        [Column(TypeName = "date")]
        [Display(Name = "Date of Birth")]
        public DateTime DateOfBirth { get; set; }

        public List<Appointment> Appointments { get; set; }
    }
}
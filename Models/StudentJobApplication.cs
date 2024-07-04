using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StudentJobApplication.Models
{
    public class StudentApplication
    {
        public int StudentID { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(15, ErrorMessage = "The phone number cannot be longer than 15 digits.")]
        [Phone]
        public string Phone { get; set; }

        public byte[] Photo { get; set; }
        public byte[] Resume { get; set; }

        public string PhotoBase64 => Photo != null ? Convert.ToBase64String(Photo) : null;
        public string ResumeBase64 => Resume != null ? Convert.ToBase64String(Resume) : null;
    }
}

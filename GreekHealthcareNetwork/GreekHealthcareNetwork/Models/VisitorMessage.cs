using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GreekHealthcareNetwork.Models
{
    public class VisitorMessage : Message
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [MinLength(2, ErrorMessage = "First name cannot be less than 2 characters")]
        [MaxLength(50, ErrorMessage = "First name cannot be more than 50 characters")]
        [RegularExpression(@"^[A-Z][a-z]*$|^[Α-Ω][α-ωάήίόέύώϊϋΐΰ]*$",
         ErrorMessage = "First Name must start with capital letter and then contain only small letters of the same language.")]
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [MinLength(2, ErrorMessage = "Last name cannot be less than 2 characters")]
        [MaxLength(50, ErrorMessage = "Last name cannot be more than 50 characters")]
        [RegularExpression(@"^[A-Z][a-z]*$|^[Α-Ω][α-ωάήίόέύώϊϋΐΰ]*$",
         ErrorMessage = "Last Name must start with capital letter and then contain only small letters of the same language.")]
        [Required]

        [Display(Name = "Last Name")]
        public string LastName { get; set; }
    }
}
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace GreekHealthcareNetwork.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MinLength(2, ErrorMessage = "First name cannot be less than 2 characters")]
        [MaxLength(50, ErrorMessage = "First name cannot be more than 50 characters")]
        [RegularExpression(@"^[A-Z][a-z]*$|^[Α-Ω][α-ωάήίόέύώϊϋΐΰ]*$",
         ErrorMessage = "First Name must start with capital letter and then contain only small letters of the same language.")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Last name cannot be less than 2 characters")]
        [MaxLength(50, ErrorMessage = "Last name cannot be more than 50 characters")]
        [RegularExpression(@"^[A-Z][a-z]*$|^[Α-Ω][α-ωάήίόέύώϊϋΐΰ]*$",
         ErrorMessage = "Last Name must start with capital letter and then contain only small letters of the same language.")]
        public string LastName { get; set; }

        [Required]
        [Column(TypeName = "date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime DoB { get; set; }

        [DefaultValue("Picturename")]
        public string ProfilePicture;

        [Required]
        [RegularExpression(@"^[0-9]{11}$",
         ErrorMessage = "AMKA should be 11 digits.")]
        public long AMKA { get; set; }

        public bool IsActive;

        [DataType(DataType.Date)]
        [Column(TypeName="date")]
        public DateTime SubscriptionEndDate { get; set; }

        public virtual ICollection<Message> Messages { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

    }
}
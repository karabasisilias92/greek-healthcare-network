using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace GreekHealthcareNetwork.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "First name cannot be less than 2 characters")]
        [MaxLength(50, ErrorMessage = "First name cannot be more than 50 characters")]
        [RegularExpression(@"^[A-Z][a-z]*$|^[Α-Ω][α-ωάήίόέύώϊϋΐΰ]*$",
         ErrorMessage = "First Name must start with capital letter and then contain only small letters of the same language.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = "Last name cannot be less than 2 characters")]
        [MaxLength(50, ErrorMessage = "Last name cannot be more than 50 characters")]
        [RegularExpression(@"^[A-Z][a-z]*$|^[Α-Ω][α-ωάήίόέύώϊϋΐΰ]*$",
         ErrorMessage = "Last Name must start with capital letter and then contain only small letters of the same language.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Telephone Number")]
        // Implement Regular Expression if there is time
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [Display(Name = "Date of Birth")]
        public DateTime DoB { get; set; }

        [Required]
        [MinLength(11, ErrorMessage = "AMKA should be 11 digits.")]
        [MaxLength(11, ErrorMessage = "AMKA should be 11 digits.")]
        [RegularExpression(@"^[0-9]{11}$",
         ErrorMessage = "AMKA should be 11 digits.")]
        [Display(Name = "AMKA/SSN")]
        public string AMKA { get; set; }

        [Display(Name = "Profile Picture")]
        public HttpPostedFileBase ProfilePicture { get; set; }

        [Required]
        [Display(Name = "Paypal Account")]
        public string PaypalAccount { get; set; }
    }

    public class DoctorRegisterViewModel : RegisterViewModel
    {
        [Required]
        [Display(Name = "Medical Specialty")]
        public MedicalSpecialty? MedicalSpecialty { get; set; }

        [Required]
        [MinLength(10, ErrorMessage = "Office Address length cannot be less than 10 characters.")]
        [MaxLength(100, ErrorMessage = "Office Address length cannot be more than 100 characters.")]
        [Display(Name = "Office Address")]
        public string OfficeAddress { get; set; }

        public List<MedicalSpecialty> MedicalSpecialties { get; set; }
    }

    public class RegisterDoctorWorkingHoursViewModel
    {
        public List<DayOfWeek> Days { get; set; }

        [Required]
        public List<WorkingHours> WorkingHours { get; set; }
        public string DoctorId { get; set; }
    }

    public class PayDoctorPlanViewModel
    {
        public DoctorPlan DoctorPlan { get; set; }
        public string DoctorId { get; set; }
    }

    public class InsuredRegisterViewModel : RegisterViewModel
    {
        [Required]
        [MinLength(10, ErrorMessage = "Home Address length cannot be less than 10 characters.")]
        [MaxLength(100, ErrorMessage = "Home Address length cannot be more than 100 characters.")]
        [Display(Name = "Home Address")]
        public string HomeAddress { get; set; }
    }

    public class PayInsuredPlanViewModel
    {
        public List<InsuredPlan> InsuredPlans { get; set; }
        public string InsuredId { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace login__pag1.Models
{
    public class User
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string   Emailid { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string otp {  get; set; }
    }
}

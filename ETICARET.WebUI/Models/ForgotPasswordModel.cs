using System.ComponentModel.DataAnnotations;

namespace ETICARET.WebUI.Models
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Eposta zorunludur.")]
        [EmailAddress(ErrorMessage ="Geçerli bir eposta girin.")]
        [Display(Name ="E-posta")]
        public string Email { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace ETICARET.WebUI.Models
{
    public class ResetPasswordModel
    { 
        // E-postadaki linkten gelen token (gizli alan olarak taşınır)
        [Required]
        public string Token { get; set; } = string.Empty;

        [Required(ErrorMessage = "E-posta zorunludur.")]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta girin.")]
        [Display(Name = "E-posta")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Yeni şifre zorunludur.")]
        [DataType(DataType.Password)]
        [Display(Name = "Yeni şifre")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Şifre tekrarı zorunludur.")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Şifreler eşleşmiyor.")]
        [Display(Name = "Yeni şifre (tekrar)")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}

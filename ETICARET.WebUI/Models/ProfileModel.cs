using System.ComponentModel.DataAnnotations;

namespace ETICARET.WebUI.Models
{
    public class ProfileModel
    {
        [Required(ErrorMessage = "Ad soyad zorunludur.")]
        [StringLength(100)]
        [Display(Name = "Ad Soyad")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kullanıcı adı zorunludur.")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "3-50 karakter arasında olmalıdır.")]
        [Display(Name = "Kullanıcı adı")]
        public string UserName { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Geçerli bir telefon numarası girin.")]
        [Display(Name = "Telefon")]
        public string? PhoneNumber { get; set; }

        // Sadece gösterilir, formdan güncellenmez.
        [Display(Name = "E-posta")]
        public string? Email { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.Business.Models
{
    public record ServiceResult(bool Success, string Message)
    {
        public static ServiceResult Ok(string message = "İşlem başarılı.") => new (true, message);
        public static ServiceResult Fail(string message) => new (false, message);
    }
}

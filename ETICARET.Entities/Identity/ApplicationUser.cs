using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETICARET.Entities.Identity
{
    public class ApplicationUser:IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}

using Microsoft.AspNetCore.Identity;
using AlexAPI.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlexAPI.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        [ForeignKey("UserId")]
        public virtual List<Yacht> Companies { get; set; }
        public bool is2FA { get; set; }
        public Guid secretKey2FA { get; set; }
    }
}

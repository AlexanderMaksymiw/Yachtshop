using Microsoft.AspNetCore.Identity;
using AlexAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlexAPI.Authentication
{
    public class ApplicationUser : IdentityUser
    {
        [ForeignKey("UserId")]
        public virtual List<UserItinerary> Itineraries { get; set; }
    }
}

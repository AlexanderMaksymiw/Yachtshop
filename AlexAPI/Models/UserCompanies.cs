using AlexAPI.Authentication;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlexAPI.Models
{
    public class UserCompanies
    {
        [ForeignKey("UserId")]
        public virtual List<ApplicationUser> User { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Yacht Company { get; set;}
    }
}

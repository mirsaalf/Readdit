using Microsoft.AspNetCore.Identity;
using Readdit.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Readdit.Areas.Identity.Data
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string? FirstName { get; set; }

        [PersonalData]
        [Column(TypeName = "nvarchar(100)")]
        public string? LastName { get; set; }

        public string? Email { get; set; }
        public DateTime UserCreatedDate { get; set; }

        // Navigation property for user-book relationship
        public ICollection<UserBook> UserBooks { get; set; }
    }
}

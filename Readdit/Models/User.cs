using Microsoft.AspNetCore.Identity;

namespace Readdit.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
        // Navigation property for the UserBooks relationship
        public ICollection<UserBook> UserBooks { get; set; }
    }
}

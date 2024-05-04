using Readdit.Areas.Identity.Data;

namespace Readdit.Models
{
    public class UserDetailsViewModel
    {
            public ApplicationUser User { get; set; }
            public IList<UserBook> UserBooks { get; set; }
            public List<Book> Books { get; set; }

        public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public DateTime UserCreatedDate { get; set; }
        
    }
}

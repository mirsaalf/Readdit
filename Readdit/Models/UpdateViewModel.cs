using Readdit.Areas.Identity.Data;

namespace Readdit.Models
{
    public class UpdateViewModel
    {
        public List<ApplicationUser> Users { get; set; }
        public List<Book> Books { get; set; }
    }

}

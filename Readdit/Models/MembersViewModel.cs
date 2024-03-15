using Readdit.Areas.Identity.Data;
using Readdit.Models;

namespace Readdit.Models
{
    public class MembersViewModel
    {
        public IEnumerable<Book> CurrentlyReadingBooks { get; set; }
        public IEnumerable<Book> ToBeReadBooks { get; set; }
        public IEnumerable<Book> CompletedReadingBooks { get; set; }
    }
}

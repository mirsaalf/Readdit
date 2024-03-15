using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Readdit.Models
{

    public enum BookStatus
    {
        CurrentlyReading,
        ToBeRead,
        Completed
    }

    public class Book
    {
       
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int book_id { get; set; }
        public string book_name { get; set; }
        public string author_name { get; set; }
        public string book_genre { get; set;}
        public DateTime date_added { get; set; }
        public string UserId { get; set; }
        public BookStatus Status { get; set; }
        // Navigation property for the UserBooks relationship
        public ICollection<UserBook> UserBooks { get; set; }
    }
}

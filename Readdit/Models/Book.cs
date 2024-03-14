using System.ComponentModel.DataAnnotations;

namespace Readdit.Models
{
    public class Book
    {
       
        [Key] public int book_id { get; set; }
        public string book_name { get; set; }
        public string author_name { get; set; }
        public string book_genre { get; set;}
    }
}

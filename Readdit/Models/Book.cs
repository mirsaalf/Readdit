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

        [Required(ErrorMessage = "The Book Name field is required.")]
        [StringLength(100)]
        public string book_name { get; set; }

        [Required(ErrorMessage = "The Author Name field is required.")]
        [StringLength(100)]
        public string author_name { get; set; }

        [Required(ErrorMessage = "The Book Genre field is required.")]
        [StringLength(50)]
        public string book_genre { get; set; }

        public DateTime date_added { get; set; } = DateTime.Now;

        // Navigation property for the UserBooks relationship
        [NotMapped]
        public ICollection<UserBook>? UserBooks { get; set; }
    }

}

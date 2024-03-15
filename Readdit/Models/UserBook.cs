using Readdit.Areas.Identity.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Readdit.Models
{
    public class UserBook
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string UserId { get; set; }  
        public User User { get; set; }
        public int BookId { get; set; }
        public Book Book { get; set; }
        public BookStatus Status { get; set; }

    }
}

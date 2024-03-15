﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Readdit.Models
{
    public class Book
    {
       
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int book_id { get; set; }
        public string book_name { get; set; }
        public string author_name { get; set; }
        public string book_genre { get; set;}
    }
}

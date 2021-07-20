using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace YourBooks.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        [DisplayName("Date of publication")]
        public string DateOfPublication { get; set; }
        public string Publisher { get; set; }
        public string CoverImage { get; set; }
        public List<Review> Reviews { get; set; }
    }
}

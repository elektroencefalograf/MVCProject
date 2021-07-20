using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourBooks.Areas.Identity.Data;

namespace YourBooks.Models
{
    public class BookReviewViewModel
    {
        public Book Book { get; set; }
        public List<Review> Reviews { get; set; }
        public ApplicationUser Author { get; set; }
        public string ReviewBody { get; set; }
        public int BookId { get; set; }
    }
}

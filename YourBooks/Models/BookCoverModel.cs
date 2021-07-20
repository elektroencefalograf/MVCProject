using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace YourBooks.Models
{
    public class BookCoverModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        [DisplayName("Date of publication")]
        public string DateOfPublication { get; set; }
        public string Publisher { get; set; }
        public List<Review> Reviews { get; set; }
        public IFormFile FileName { get; set; }
    }
}

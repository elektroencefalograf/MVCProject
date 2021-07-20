using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using YourBooks.Areas.Identity.Data;

namespace YourBooks.Models
{
    public class Review
    {
        public int Id { get; set; }
        [DisplayName(" ")]
        public string ReviewBody { get; set; }
        public virtual ApplicationUser Author { get; set; }
        public virtual Book Book { get; set; }
    }
}

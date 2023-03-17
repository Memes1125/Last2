using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.DB
{
    public partial class Book
    {
        [NotMapped]
        public string BookAuthor { get; set; }
        
        [NotMapped]
        public string BookGenre { get; set; }
        
        [NotMapped]
        public string BookShelf { get; set; }

        [NotMapped]
        public decimal? FullPrice { get; set; }

        [NotMapped]
        public List<Author> Authors { get; set; } 

        [NotMapped]
        public List<Genre> Genres { get; set; }
    }
}

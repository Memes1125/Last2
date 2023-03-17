using System;
using System.Collections.Generic;

namespace LibraryProject.DB
{
    public partial class Author
    {
        public Author()
        {
            BookAuthorCrosses = new HashSet<BookAuthorCross>();
        }

        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Patronimyc { get; set; }

        public virtual ICollection<BookAuthorCross> BookAuthorCrosses { get; set; }
    }
}

using System;
using System.Collections.Generic;

namespace LibraryProject.DB
{
    public partial class BookAuthorCross
    {
        public int BookId { get; set; }
        public int AuthorId { get; set; }
        public string? FullName { get; set; }

        public virtual Author Author { get; set; } = null!;
        public virtual Book Book { get; set; } = null!;
    }
}

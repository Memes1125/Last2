using System;
using System.Collections.Generic;

namespace LibraryProject.DB
{
    public partial class Shelf
    {
        public Shelf()
        {
            Books = new HashSet<Book>();
        }

        public int Id { get; set; }
        public string? ShelfNumber { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}

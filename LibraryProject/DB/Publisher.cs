using System;
using System.Collections.Generic;

namespace LibraryProject.DB
{
    public partial class Publisher
    {
        public Publisher()
        {
            Books = new HashSet<Book>();
        }

        public int Id { get; set; }
        public string? PublisherName { get; set; }
        public string? PublisherLocation { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}

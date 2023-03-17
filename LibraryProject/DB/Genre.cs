using System;
using System.Collections.Generic;

namespace LibraryProject.DB
{
    public partial class Genre
    {
        public Genre()
        {
            BookGenreCrosses = new HashSet<BookGenreCross>();
        }

        public int Id { get; set; }
        public string? GenreName { get; set; }

        public virtual ICollection<BookGenreCross> BookGenreCrosses { get; set; }
    }
}

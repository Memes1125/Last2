using System;
using System.Collections.Generic;

namespace LibraryProject.DB
{
    public partial class BookGenreCross
    {
        public int BookId { get; set; }
        public int GenreId { get; set; }
        public int? Id { get; set; }

        public virtual Book Book { get; set; } = null!;
        public virtual Genre Genre { get; set; } = null!;
    }
}

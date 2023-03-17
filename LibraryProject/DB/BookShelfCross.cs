using System;
using System.Collections.Generic;

namespace LibraryProject.DB
{
    public partial class BookShelfCross
    {
        public int BookId { get; set; }
        public int ShelfId { get; set; }
        public int? CountBook { get; set; }

        public virtual Book Book { get; set; } = null!;
        public virtual Shelf Shelf { get; set; } = null!;
    }
}

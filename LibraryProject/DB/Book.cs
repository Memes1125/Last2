using System;
using System.Collections.Generic;

namespace LibraryProject.DB
{
    public partial class Book
    {
        public Book()
        {
            BookAuthorCrosses = new HashSet<BookAuthorCross>();
            BookGenreCrosses = new HashSet<BookGenreCross>();
        }

        public int Id { get; set; }
        public string? BookName { get; set; }
        public int? Count { get; set; }
        public int? PublisherId { get; set; }
        public decimal? Price { get; set; }
        public int? DepartamentId { get; set; }
        public DateTime? YearPublished { get; set; }
        public int? ShelfId { get; set; }

        public virtual Department? Departament { get; set; }
        public virtual Publisher? Publisher { get; set; }
        public virtual Shelf? Shelf { get; set; }
        public virtual ICollection<BookAuthorCross> BookAuthorCrosses { get; set; }
        public virtual ICollection<BookGenreCross> BookGenreCrosses { get; set; }
    }
}

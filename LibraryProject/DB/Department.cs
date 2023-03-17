using System;
using System.Collections.Generic;

namespace LibraryProject.DB
{
    public partial class Department
    {
        public Department()
        {
            Books = new HashSet<Book>();
        }

        public int Id { get; set; }
        public string? DepartmentName { get; set; }

        public virtual ICollection<Book> Books { get; set; }
    }
}

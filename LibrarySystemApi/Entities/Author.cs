using System;
using System.Collections.Generic;

namespace LibrarySystemApi.Entities
{
    public class Author : BaseEntity
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }

        public virtual ICollection<Book> Books { get; set; } = new HashSet<Book>();
    }
}

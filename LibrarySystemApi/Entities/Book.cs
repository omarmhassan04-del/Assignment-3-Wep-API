using System.Collections.Generic;

namespace LibrarySystemApi.Entities
{
    public class Book : BaseEntity
    {
        public string Title { get; set; }
        public string ISBN { get; set; }

        public int AuthorId { get; set; }
        public virtual Author Author { get; set; }

        public virtual ICollection<Loan> Loans { get; set; } = new HashSet<Loan>();
    }
}

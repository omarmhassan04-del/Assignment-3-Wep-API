using System;

namespace LibrarySystemApi.Entities
{
    public class Loan
    {
        public int BookId { get; set; }
        public virtual Book Book { get; set; }

        public int BorrowerId { get; set; }
        public virtual Borrower Borrower { get; set; }

        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }

        public bool IsDeleted { get; set; } = false;
        public DateTime? CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}

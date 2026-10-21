using System;

namespace LibrarySystemApi.Dtos.LoanDtos
{
    public class CreateLoanRequest
    {
        public int BookId { get; set; }
        public int BorrowerId { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}

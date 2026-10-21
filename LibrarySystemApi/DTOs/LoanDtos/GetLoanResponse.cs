using System;

namespace LibrarySystemApi.Dtos.LoanDtos
{
    public class GetLoanResponse
    {
        public int BookId { get; set; }
        public string BookTitle { get; set; }
        public int BorrowerId { get; set; }
        public string BorrowerName { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}

using System;
using System.Collections.Generic;
using LibrarySystemApi.Dtos.LoanDtos;

namespace LibrarySystemApi.Dtos.BorrowerDtos
{
    public class GetBorrowerResponse
    {
        public int BorrowerId { get; set; }
        public string BorrowerName { get; set; }
        public DateTime MembershipDate { get; set; }
        public List<GetLoanResponse> Loans { get; set; } = new List<GetLoanResponse>();
    }
}

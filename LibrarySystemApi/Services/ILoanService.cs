using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibrarySystemApi.Dtos.LoanDtos;
using LibrarySystemApi.Results;

namespace LibrarySystemApi.Services
{
    public interface ILoanService
    {
        Task<GenericResult<IEnumerable<GetLoanResponse>>> GetAllAsync();
        Task<GenericResult<PaginatedList<GetLoanResponse>>> GetAllAsync(int pageNumber, int pageSize, string searchTerm = null);
        Task<GenericResult<GetLoanResponse>> GetByIdAsync(int bookId, int borrowerId, DateTime loanDate);
        Task<GenericResult<GetLoanResponse>> CreateAsync(CreateLoanRequest request);
        Task<GenericResult<bool>> UpdateAsync(UpdateLoanRequest request);
        Task<GenericResult<bool>> DeleteAsync(int bookId, int borrowerId, DateTime loanDate);
    }
}

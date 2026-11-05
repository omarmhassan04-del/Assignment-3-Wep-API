using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibrarySystemApi.Entities;
using LibrarySystemApi.Results;

namespace LibrarySystemApi.Repositories
{
    public interface ILoanRepository
    {
        Task<IEnumerable<Loan>> GetAllAsync();
        Task<PaginatedList<Loan>> GetAllAsync(int pageNumber, int pageSize, string searchTerm = null);
        Task<Loan> GetByIdAsync(int bookId, int borrowerId, DateTime loanDate);
        Task<Loan> CreateAsync(Loan loan);
        Task<bool> UpdateAsync(Loan loan);
        Task<bool> DeleteAsync(int bookId, int borrowerId, DateTime loanDate);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LibrarySystemApi.Data;
using LibrarySystemApi.Entities;
using LibrarySystemApi.Results;

namespace LibrarySystemApi.Repositories
{
    public class LoanRepository : ILoanRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public LoanRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Loan>> GetAllAsync()
        {
            return await _dbContext.Loans
                                   .Include(l => l.Book)
                                   .Include(l => l.Borrower)
                                   .AsNoTracking()
                                   .ToListAsync();
        }

        public async Task<PaginatedList<Loan>> GetAllAsync(int pageNumber, int pageSize, string searchTerm = null)
        {
            IQueryable<Loan> query = _dbContext.Loans
                                               .Include(l => l.Book)
                                               .Include(l => l.Borrower)
                                               .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(l => l.Book.Title.Contains(searchTerm) || l.Borrower.Name.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var loans = await query.Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            return new PaginatedList<Loan>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Data = loans
            };
        }

        public async Task<Loan> GetByIdAsync(int bookId, int borrowerId, DateTime loanDate)
        {
            return await _dbContext.Loans
                                   .Include(l => l.Book)
                                   .Include(l => l.Borrower)
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync(l => l.BookId == bookId && l.BorrowerId == borrowerId && l.LoanDate == loanDate);
        }

        public async Task<Loan> CreateAsync(Loan loan)
        {
            await _dbContext.Loans.AddAsync(loan);
            await _dbContext.SaveChangesAsync();
            return loan;
        }

        public async Task<bool> UpdateAsync(Loan loan)
        {
            var existing = await _dbContext.Loans.FindAsync(loan.BookId, loan.BorrowerId, loan.LoanDate);
            if (existing is null)
                return false;

            existing.ReturnDate = loan.ReturnDate;
            existing.ModifiedAt = DateTime.UtcNow;

            _dbContext.Loans.Update(existing);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int bookId, int borrowerId, DateTime loanDate)
        {
            var existing = await _dbContext.Loans.FindAsync(bookId, borrowerId, loanDate);
            if (existing is null)
                return false;

            existing.IsDeleted = true;
            existing.ModifiedAt = DateTime.UtcNow;

            _dbContext.Loans.Update(existing);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}

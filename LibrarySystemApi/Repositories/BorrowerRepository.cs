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
    public class BorrowerRepository : IBorrowerRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BorrowerRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Borrower>> GetAllAsync()
        {
            return await _dbContext.Borrowers
                                   .Include(b => b.Loans)
                                        .ThenInclude(l => l.Book)
                                   .AsNoTracking()
                                   .ToListAsync();
        }

        public async Task<PaginatedList<Borrower>> GetAllAsync(int pageNumber, int pageSize, string searchTerm = null)
        {
            IQueryable<Borrower> query = _dbContext.Borrowers
                                                   .Include(b => b.Loans)
                                                        .ThenInclude(l => l.Book)
                                                   .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(b => b.Name.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var borrowers = await query.Skip((pageNumber - 1) * pageSize)
                                       .Take(pageSize)
                                       .ToListAsync();

            return new PaginatedList<Borrower>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Data = borrowers
            };
        }

        public async Task<Borrower> GetByIdAsync(int id)
        {
            return await _dbContext.Borrowers
                                   .Include(b => b.Loans)
                                        .ThenInclude(l => l.Book)
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Borrower> CreateAsync(Borrower borrower)
        {
            await _dbContext.Borrowers.AddAsync(borrower);
            await _dbContext.SaveChangesAsync();
            return borrower;
        }

        public async Task<bool> UpdateAsync(Borrower borrower)
        {
            var existing = await _dbContext.Borrowers.FindAsync(borrower.Id);
            if (existing is null)
                return false;

            existing.Name = borrower.Name;
            existing.MembershipDate = borrower.MembershipDate;
            existing.ModifiedAt = DateTime.UtcNow;

            _dbContext.Borrowers.Update(existing);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var borrower = await _dbContext.Borrowers.FindAsync(id);
            if (borrower is null)
                return false;

            borrower.IsDeleted = true;
            borrower.ModifiedAt = DateTime.UtcNow;

            _dbContext.Borrowers.Update(borrower);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}

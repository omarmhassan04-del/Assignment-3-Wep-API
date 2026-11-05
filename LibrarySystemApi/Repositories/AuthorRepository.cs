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
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AuthorRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            return await _dbContext.Authors
                                   .Include(a => a.Books)
                                   .AsNoTracking()
                                   .ToListAsync();
        }

        public async Task<PaginatedList<Author>> GetAllAsync(int pageNumber, int pageSize, string searchTerm = null)
        {
            IQueryable<Author> query = _dbContext.Authors
                                                 .Include(a => a.Books)
                                                 .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(a => a.Name.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var authors = await query.Skip((pageNumber - 1) * pageSize)
                                     .Take(pageSize)
                                     .ToListAsync();

            return new PaginatedList<Author>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Data = authors
            };
        }

        public async Task<Author> GetByIdAsync(int id)
        {
            return await _dbContext.Authors
                                   .Include(a => a.Books)
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Author> CreateAsync(Author author)
        {
            await _dbContext.Authors.AddAsync(author);
            await _dbContext.SaveChangesAsync();
            return author;
        }

        public async Task<bool> UpdateAsync(Author author)
        {
            var existing = await _dbContext.Authors.FindAsync(author.Id);
            if (existing is null)
                return false;

            existing.Name = author.Name;
            existing.BirthDate = author.BirthDate;
            existing.ModifiedAt = DateTime.UtcNow;

            _dbContext.Authors.Update(existing);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var author = await _dbContext.Authors.FindAsync(id);
            if (author is null)
                return false;

            author.IsDeleted = true;
            author.ModifiedAt = DateTime.UtcNow;

            _dbContext.Authors.Update(author);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}

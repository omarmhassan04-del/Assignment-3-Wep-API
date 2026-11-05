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
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public BookRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _dbContext.Books
                                   .Include(b => b.Author)
                                   .AsNoTracking()
                                   .ToListAsync();
        }

        public async Task<PaginatedList<Book>> GetAllAsync(int pageNumber, int pageSize, string searchTerm = null)
        {
            IQueryable<Book> query = _dbContext.Books
                                               .Include(b => b.Author)
                                               .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(b => b.Title.Contains(searchTerm) || b.ISBN.Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();

            var books = await query.Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            return new PaginatedList<Book>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                Data = books
            };
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            return await _dbContext.Books
                                   .Include(b => b.Author)
                                   .AsNoTracking()
                                   .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<Book> CreateAsync(Book book)
        {
            await _dbContext.Books.AddAsync(book);
            await _dbContext.SaveChangesAsync();
            return book;
        }

        public async Task<bool> UpdateAsync(Book book)
        {
            var existing = await _dbContext.Books.FindAsync(book.Id);
            if (existing is null)
                return false;

            existing.Title = book.Title;
            existing.ISBN = book.ISBN;
            existing.AuthorId = book.AuthorId;
            existing.ModifiedAt = DateTime.UtcNow;

            _dbContext.Books.Update(existing);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var book = await _dbContext.Books.FindAsync(id);
            if (book is null)
                return false;

            book.IsDeleted = true;
            book.ModifiedAt = DateTime.UtcNow;

            _dbContext.Books.Update(book);
            await _dbContext.SaveChangesAsync();
            return true;
        }
    }
}

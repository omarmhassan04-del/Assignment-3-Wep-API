using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibrarySystemApi.Data;
using LibrarySystemApi.Dtos.BookDtos;
using LibrarySystemApi.Entities;

namespace LibrarySystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public BooksController(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet("GetAllBooks")]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _dbContext.Books
                                       .Include(b => b.Author)
                                       .AsNoTracking()
                                       .ToListAsync();

            var response = _mapper.Map<List<Book>, List<GetBookResponse>>(books);
            return Ok(response);
        }

        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _dbContext.Books
                                      .Include(b => b.Author)
                                      .AsNoTracking()
                                      .FirstOrDefaultAsync(b => b.Id == id);

            if (book is null)
                return NotFound($"Book with Id = {id} is not found.");

            var response = _mapper.Map<Book, GetBookResponse>(book);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookRequest request)
        {
            var author = await _dbContext.Authors.FindAsync(request.AuthorId);
            if (author is null)
                return NotFound($"Author with Id = {request.AuthorId} is not found.");

            var book = new Book
            {
                Title = request.Title,
                ISBN = request.ISBN,
                AuthorId = request.AuthorId,
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.Books.AddAsync(book);
            await _dbContext.SaveChangesAsync();
            return Created();
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateBookRequest request)
        {
            var book = await _dbContext.Books.FindAsync(request.Id);

            if (book is null)
                return NotFound($"Book with Id = {request.Id} is not found.");

            var author = await _dbContext.Authors.FindAsync(request.AuthorId);
            if (author is null)
                return NotFound($"Author with Id = {request.AuthorId} is not found.");

            book.Title = request.Title;
            book.ISBN = request.ISBN;
            book.AuthorId = request.AuthorId;
            book.ModifiedAt = DateTime.UtcNow;

            _dbContext.Books.Update(book);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Remove(int id)
        {
            var book = await _dbContext.Books.FindAsync(id);

            if (book is null)
                return NotFound($"Book with Id = {id} is not found.");

            book.IsDeleted = true;
            book.ModifiedAt = DateTime.UtcNow;

            _dbContext.Books.Update(book);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}

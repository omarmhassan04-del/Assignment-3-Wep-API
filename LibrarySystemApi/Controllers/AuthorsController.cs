using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibrarySystemApi.Data;
using LibrarySystemApi.Dtos.AuthorDtos;
using LibrarySystemApi.Entities;

namespace LibrarySystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public AuthorsController(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet("GetAllAuthors")]
        public async Task<IActionResult> GetAllAuthors()
        {
            var authors = await _dbContext.Authors
                                         .Include(a => a.Books)
                                         .AsNoTracking()
                                         .ToListAsync();

            var response = _mapper.Map<List<Author>, List<GetAuthorResponse>>(authors);
            return Ok(response);
        }

        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var author = await _dbContext.Authors
                                        .Include(a => a.Books)
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(a => a.Id == id);

            if (author is null)
                return NotFound($"Author with Id = {id} is not found.");

            var response = _mapper.Map<Author, GetAuthorResponse>(author);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateAuthorRequest request)
        {
            var author = new Author
            {
                Name = request.Name,
                BirthDate = request.BirthDate,
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.Authors.AddAsync(author);
            await _dbContext.SaveChangesAsync();
            return Created();
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateAuthorRequest request)
        {
            var author = await _dbContext.Authors.FindAsync(request.Id);

            if (author is null)
                return NotFound($"Author with Id = {request.Id} is not found.");

            author.Name = request.Name;
            author.BirthDate = request.BirthDate;
            author.ModifiedAt = DateTime.UtcNow;

            _dbContext.Authors.Update(author);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Remove(int id)
        {
            var author = await _dbContext.Authors.FindAsync(id);

            if (author is null)
                return NotFound($"Author with Id = {id} is not found.");

            author.IsDeleted = true;
            author.ModifiedAt = DateTime.UtcNow;

            _dbContext.Authors.Update(author);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}

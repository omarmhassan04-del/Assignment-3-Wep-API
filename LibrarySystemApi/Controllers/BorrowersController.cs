using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibrarySystemApi.Data;
using LibrarySystemApi.Dtos.BorrowerDtos;
using LibrarySystemApi.Entities;

namespace LibrarySystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowersController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public BorrowersController(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet("GetAllBorrowers")]
        public async Task<IActionResult> GetAllBorrowers()
        {
            var borrowers = await _dbContext.Borrowers
                                           .Include(b => b.Loans)
                                                .ThenInclude(l => l.Book)
                                           .AsNoTracking()
                                           .ToListAsync();

            var response = _mapper.Map<List<Borrower>, List<GetBorrowerResponse>>(borrowers);
            return Ok(response);
        }

        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var borrower = await _dbContext.Borrowers
                                          .Include(b => b.Loans)
                                                .ThenInclude(l => l.Book)
                                          .AsNoTracking()
                                          .FirstOrDefaultAsync(b => b.Id == id);

            if (borrower is null)
                return NotFound($"Borrower with Id = {id} is not found.");

            var response = _mapper.Map<Borrower, GetBorrowerResponse>(borrower);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBorrowerRequest request)
        {
            var borrower = new Borrower
            {
                Name = request.Name,
                MembershipDate = request.MembershipDate,
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.Borrowers.AddAsync(borrower);
            await _dbContext.SaveChangesAsync();
            return Created();
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateBorrowerRequest request)
        {
            var borrower = await _dbContext.Borrowers.FindAsync(request.Id);

            if (borrower is null)
                return NotFound($"Borrower with Id = {request.Id} is not found.");

            borrower.Name = request.Name;
            borrower.MembershipDate = request.MembershipDate;
            borrower.ModifiedAt = DateTime.UtcNow;

            _dbContext.Borrowers.Update(borrower);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Remove(int id)
        {
            var borrower = await _dbContext.Borrowers.FindAsync(id);

            if (borrower is null)
                return NotFound($"Borrower with Id = {id} is not found.");

            borrower.IsDeleted = true;
            borrower.ModifiedAt = DateTime.UtcNow;

            _dbContext.Borrowers.Update(borrower);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}

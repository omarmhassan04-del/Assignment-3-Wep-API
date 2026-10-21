using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LibrarySystemApi.Data;
using LibrarySystemApi.Dtos.LoanDtos;
using LibrarySystemApi.Entities;

namespace LibrarySystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IMapper _mapper;

        public LoansController(ApplicationDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [HttpGet("GetAllLoans")]
        public async Task<IActionResult> GetAllLoans()
        {
            var loans = await _dbContext.Loans
                                       .Include(l => l.Book)
                                       .Include(l => l.Borrower)
                                       .AsNoTracking()
                                       .ToListAsync();

            var response = _mapper.Map<List<Loan>, List<GetLoanResponse>>(loans);
            return Ok(response);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int bookId, int borrowerId, DateTime loanDate)
        {
            var loan = await _dbContext.Loans
                                      .Include(l => l.Book)
                                      .Include(l => l.Borrower)
                                      .AsNoTracking()
                                      .FirstOrDefaultAsync(l => l.BookId == bookId && l.BorrowerId == borrowerId && l.LoanDate == loanDate);

            if (loan is null)
                return NotFound("Loan record is not found.");

            var response = _mapper.Map<Loan, GetLoanResponse>(loan);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateLoanRequest request)
        {
            var book = await _dbContext.Books.FindAsync(request.BookId);
            if (book is null)
                return NotFound($"Book with Id = {request.BookId} is not found.");

            var borrower = await _dbContext.Borrowers.FindAsync(request.BorrowerId);
            if (borrower is null)
                return NotFound($"Borrower with Id = {request.BorrowerId} is not found.");

            var existing = await _dbContext.Loans.FindAsync(request.BookId, request.BorrowerId, request.LoanDate);
            if (existing is not null)
                return Conflict("A loan record with the same key already exists.");

            var loan = new Loan
            {
                BookId = request.BookId,
                BorrowerId = request.BorrowerId,
                LoanDate = request.LoanDate,
                ReturnDate = request.ReturnDate,
                CreatedAt = DateTime.UtcNow
            };

            await _dbContext.Loans.AddAsync(loan);
            await _dbContext.SaveChangesAsync();
            return Created();
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateLoanRequest request)
        {
            var loan = await _dbContext.Loans.FindAsync(request.BookId, request.BorrowerId, request.LoanDate);

            if (loan is null)
                return NotFound("Loan record is not found.");

            loan.ReturnDate = request.ReturnDate;
            loan.ModifiedAt = DateTime.UtcNow;

            _dbContext.Loans.Update(loan);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        public async Task<IActionResult> Remove(int bookId, int borrowerId, DateTime loanDate)
        {
            var loan = await _dbContext.Loans.FindAsync(bookId, borrowerId, loanDate);

            if (loan is null)
                return NotFound("Loan record is not found.");

            loan.IsDeleted = true;
            loan.ModifiedAt = DateTime.UtcNow;

            _dbContext.Loans.Update(loan);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}

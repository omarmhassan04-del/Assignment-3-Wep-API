using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibrarySystemApi.Dtos.LoanDtos;
using LibrarySystemApi.Services;

namespace LibrarySystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _service;

        public LoansController(ILoanService service)
        {
            _service = service;
        }

        [HttpGet("GetAllLoans")]
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10, string searchTerm = null)
        {
            var result = await _service.GetAllAsync(pageNumber, pageSize, searchTerm);
            return Ok(result);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int bookId, int borrowerId, DateTime loanDate)
        {
            var result = await _service.GetByIdAsync(bookId, borrowerId, loanDate);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateLoanRequest request)
        {
            var result = await _service.CreateAsync(request);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateLoanRequest request)
        {
            var result = await _service.UpdateAsync(request);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Remove(int bookId, int borrowerId, DateTime loanDate)
        {
            var result = await _service.DeleteAsync(bookId, borrowerId, loanDate);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
    }
}

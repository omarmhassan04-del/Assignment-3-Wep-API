using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibrarySystemApi.Dtos.BookDtos;
using LibrarySystemApi.Services;

namespace LibrarySystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _service;

        public BooksController(IBookService service)
        {
            _service = service;
        }

        [HttpGet("GetAllBooks")]
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10, string searchTerm = null)
        {
            var result = await _service.GetAllAsync(pageNumber, pageSize, searchTerm);
            return Ok(result);
        }

        [HttpGet("GetById/{id:int}")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateBookRequest request)
        {
            var result = await _service.CreateAsync(request);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateBookRequest request)
        {
            var result = await _service.UpdateAsync(request);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }

        [HttpDelete]
        public async Task<IActionResult> Remove(int id)
        {
            var result = await _service.DeleteAsync(id);
            return result.IsSuccess ? Ok(result) : NotFound(result);
        }
    }
}

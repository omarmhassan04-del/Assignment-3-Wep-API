using System.Collections.Generic;
using System.Threading.Tasks;
using LibrarySystemApi.Dtos.BookDtos;
using LibrarySystemApi.Results;

namespace LibrarySystemApi.Services
{
    public interface IBookService
    {
        Task<GenericResult<IEnumerable<GetBookResponse>>> GetAllAsync();
        Task<GenericResult<PaginatedList<GetBookResponse>>> GetAllAsync(int pageNumber, int pageSize, string searchTerm = null);
        Task<GenericResult<GetBookResponse>> GetByIdAsync(int id);
        Task<GenericResult<GetBookResponse>> CreateAsync(CreateBookRequest request);
        Task<GenericResult<bool>> UpdateAsync(UpdateBookRequest request);
        Task<GenericResult<bool>> DeleteAsync(int id);
    }
}

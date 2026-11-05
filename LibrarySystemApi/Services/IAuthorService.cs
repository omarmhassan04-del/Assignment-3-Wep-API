using System.Collections.Generic;
using System.Threading.Tasks;
using LibrarySystemApi.Dtos.AuthorDtos;
using LibrarySystemApi.Results;

namespace LibrarySystemApi.Services
{
    public interface IAuthorService
    {
        Task<GenericResult<IEnumerable<GetAuthorResponse>>> GetAllAsync();
        Task<GenericResult<PaginatedList<GetAuthorResponse>>> GetAllAsync(int pageNumber, int pageSize, string searchTerm = null);
        Task<GenericResult<GetAuthorResponse>> GetByIdAsync(int id);
        Task<GenericResult<GetAuthorResponse>> CreateAsync(CreateAuthorRequest request);
        Task<GenericResult<bool>> UpdateAsync(UpdateAuthorRequest request);
        Task<GenericResult<bool>> DeleteAsync(int id);
    }
}

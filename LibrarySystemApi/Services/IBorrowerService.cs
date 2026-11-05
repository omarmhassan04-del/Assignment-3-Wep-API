using System.Collections.Generic;
using System.Threading.Tasks;
using LibrarySystemApi.Dtos.BorrowerDtos;
using LibrarySystemApi.Results;

namespace LibrarySystemApi.Services
{
    public interface IBorrowerService
    {
        Task<GenericResult<IEnumerable<GetBorrowerResponse>>> GetAllAsync();
        Task<GenericResult<PaginatedList<GetBorrowerResponse>>> GetAllAsync(int pageNumber, int pageSize, string searchTerm = null);
        Task<GenericResult<GetBorrowerResponse>> GetByIdAsync(int id);
        Task<GenericResult<GetBorrowerResponse>> CreateAsync(CreateBorrowerRequest request);
        Task<GenericResult<bool>> UpdateAsync(UpdateBorrowerRequest request);
        Task<GenericResult<bool>> DeleteAsync(int id);
    }
}

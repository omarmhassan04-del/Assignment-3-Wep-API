using System.Collections.Generic;
using System.Threading.Tasks;
using LibrarySystemApi.Entities;
using LibrarySystemApi.Results;

namespace LibrarySystemApi.Repositories
{
    public interface IAuthorRepository
    {
        Task<IEnumerable<Author>> GetAllAsync();
        Task<PaginatedList<Author>> GetAllAsync(int pageNumber, int pageSize, string searchTerm = null);
        Task<Author> GetByIdAsync(int id);
        Task<Author> CreateAsync(Author author);
        Task<bool> UpdateAsync(Author author);
        Task<bool> DeleteAsync(int id);
    }
}

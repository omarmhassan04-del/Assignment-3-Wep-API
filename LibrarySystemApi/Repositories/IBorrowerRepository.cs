using System.Collections.Generic;
using System.Threading.Tasks;
using LibrarySystemApi.Entities;
using LibrarySystemApi.Results;

namespace LibrarySystemApi.Repositories
{
    public interface IBorrowerRepository
    {
        Task<IEnumerable<Borrower>> GetAllAsync();
        Task<PaginatedList<Borrower>> GetAllAsync(int pageNumber, int pageSize, string searchTerm = null);
        Task<Borrower> GetByIdAsync(int id);
        Task<Borrower> CreateAsync(Borrower borrower);
        Task<bool> UpdateAsync(Borrower borrower);
        Task<bool> DeleteAsync(int id);
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LibrarySystemApi.Dtos.BorrowerDtos;
using LibrarySystemApi.Entities;
using LibrarySystemApi.Repositories;
using LibrarySystemApi.Results;

namespace LibrarySystemApi.Services
{
    public class BorrowerService : IBorrowerService
    {
        private readonly IBorrowerRepository _repository;
        private readonly IMapper _mapper;

        public BorrowerService(IBorrowerRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GenericResult<IEnumerable<GetBorrowerResponse>>> GetAllAsync()
        {
            var borrowers = await _repository.GetAllAsync();
            var response = _mapper.Map<List<GetBorrowerResponse>>(borrowers);
            return GenericResult<IEnumerable<GetBorrowerResponse>>.Success(response);
        }

        public async Task<GenericResult<PaginatedList<GetBorrowerResponse>>> GetAllAsync(int pageNumber, int pageSize, string searchTerm = null)
        {
            var paginatedBorrowers = await _repository.GetAllAsync(pageNumber, pageSize, searchTerm);
            var responseList = _mapper.Map<List<GetBorrowerResponse>>(paginatedBorrowers.Data);

            var paginatedResponse = new PaginatedList<GetBorrowerResponse>
            {
                PageNumber = paginatedBorrowers.PageNumber,
                PageSize = paginatedBorrowers.PageSize,
                TotalRecords = paginatedBorrowers.TotalRecords,
                TotalPages = paginatedBorrowers.TotalPages,
                Data = responseList
            };

            return GenericResult<PaginatedList<GetBorrowerResponse>>.Success(paginatedResponse);
        }

        public async Task<GenericResult<GetBorrowerResponse>> GetByIdAsync(int id)
        {
            var borrower = await _repository.GetByIdAsync(id);
            if (borrower is null)
                return GenericResult<GetBorrowerResponse>.Failure($"Borrower with Id = {id} is not found.");

            var response = _mapper.Map<GetBorrowerResponse>(borrower);
            return GenericResult<GetBorrowerResponse>.Success(response);
        }

        public async Task<GenericResult<GetBorrowerResponse>> CreateAsync(CreateBorrowerRequest request)
        {
            var borrower = new Borrower
            {
                Name = request.Name,
                MembershipDate = request.MembershipDate,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _repository.CreateAsync(borrower);
            var response = _mapper.Map<GetBorrowerResponse>(created);
            return GenericResult<GetBorrowerResponse>.Success(response);
        }

        public async Task<GenericResult<bool>> UpdateAsync(UpdateBorrowerRequest request)
        {
            var existing = await _repository.GetByIdAsync(request.Id);
            if (existing is null)
                return GenericResult<bool>.Failure($"Borrower with Id = {request.Id} is not found.");

            existing.Name = request.Name;
            existing.MembershipDate = request.MembershipDate;

            var updated = await _repository.UpdateAsync(existing);
            return updated
                ? GenericResult<bool>.Success(true)
                : GenericResult<bool>.Failure("Failed to update borrower.");
        }

        public async Task<GenericResult<bool>> DeleteAsync(int id)
        {
            var isDeleted = await _repository.DeleteAsync(id);
            return isDeleted
                ? GenericResult<bool>.Success(true)
                : GenericResult<bool>.Failure($"Borrower with Id = {id} is not found.");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LibrarySystemApi.Dtos.AuthorDtos;
using LibrarySystemApi.Entities;
using LibrarySystemApi.Repositories;
using LibrarySystemApi.Results;

namespace LibrarySystemApi.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _repository;
        private readonly IMapper _mapper;

        public AuthorService(IAuthorRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<GenericResult<IEnumerable<GetAuthorResponse>>> GetAllAsync()
        {
            var authors = await _repository.GetAllAsync();
            var response = _mapper.Map<List<GetAuthorResponse>>(authors);
            return GenericResult<IEnumerable<GetAuthorResponse>>.Success(response);
        }

        public async Task<GenericResult<PaginatedList<GetAuthorResponse>>> GetAllAsync(int pageNumber, int pageSize, string searchTerm = null)
        {
            var paginatedAuthors = await _repository.GetAllAsync(pageNumber, pageSize, searchTerm);
            var responseList = _mapper.Map<List<GetAuthorResponse>>(paginatedAuthors.Data);

            var paginatedResponse = new PaginatedList<GetAuthorResponse>
            {
                PageNumber = paginatedAuthors.PageNumber,
                PageSize = paginatedAuthors.PageSize,
                TotalRecords = paginatedAuthors.TotalRecords,
                TotalPages = paginatedAuthors.TotalPages,
                Data = responseList
            };

            return GenericResult<PaginatedList<GetAuthorResponse>>.Success(paginatedResponse);
        }

        public async Task<GenericResult<GetAuthorResponse>> GetByIdAsync(int id)
        {
            var author = await _repository.GetByIdAsync(id);
            if (author is null)
                return GenericResult<GetAuthorResponse>.Failure($"Author with Id = {id} is not found.");

            var response = _mapper.Map<GetAuthorResponse>(author);
            return GenericResult<GetAuthorResponse>.Success(response);
        }

        public async Task<GenericResult<GetAuthorResponse>> CreateAsync(CreateAuthorRequest request)
        {
            var author = new Author
            {
                Name = request.Name,
                BirthDate = request.BirthDate,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _repository.CreateAsync(author);
            var response = _mapper.Map<GetAuthorResponse>(created);
            return GenericResult<GetAuthorResponse>.Success(response);
        }

        public async Task<GenericResult<bool>> UpdateAsync(UpdateAuthorRequest request)
        {
            var existing = await _repository.GetByIdAsync(request.Id);
            if (existing is null)
                return GenericResult<bool>.Failure($"Author with Id = {request.Id} is not found.");

            existing.Name = request.Name;
            existing.BirthDate = request.BirthDate;

            var updated = await _repository.UpdateAsync(existing);
            return updated
                ? GenericResult<bool>.Success(true)
                : GenericResult<bool>.Failure("Failed to update author.");
        }

        public async Task<GenericResult<bool>> DeleteAsync(int id)
        {
            var isDeleted = await _repository.DeleteAsync(id);
            return isDeleted
                ? GenericResult<bool>.Success(true)
                : GenericResult<bool>.Failure($"Author with Id = {id} is not found.");
        }
    }
}

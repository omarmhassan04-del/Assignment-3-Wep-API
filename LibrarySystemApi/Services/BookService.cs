using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LibrarySystemApi.Dtos.BookDtos;
using LibrarySystemApi.Entities;
using LibrarySystemApi.Repositories;
using LibrarySystemApi.Results;

namespace LibrarySystemApi.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository, IMapper mapper)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _mapper = mapper;
        }

        public async Task<GenericResult<IEnumerable<GetBookResponse>>> GetAllAsync()
        {
            var books = await _bookRepository.GetAllAsync();
            var response = _mapper.Map<List<GetBookResponse>>(books);
            return GenericResult<IEnumerable<GetBookResponse>>.Success(response);
        }

        public async Task<GenericResult<PaginatedList<GetBookResponse>>> GetAllAsync(int pageNumber, int pageSize, string searchTerm = null)
        {
            var paginatedBooks = await _bookRepository.GetAllAsync(pageNumber, pageSize, searchTerm);
            var responseList = _mapper.Map<List<GetBookResponse>>(paginatedBooks.Data);

            var paginatedResponse = new PaginatedList<GetBookResponse>
            {
                PageNumber = paginatedBooks.PageNumber,
                PageSize = paginatedBooks.PageSize,
                TotalRecords = paginatedBooks.TotalRecords,
                TotalPages = paginatedBooks.TotalPages,
                Data = responseList
            };

            return GenericResult<PaginatedList<GetBookResponse>>.Success(paginatedResponse);
        }

        public async Task<GenericResult<GetBookResponse>> GetByIdAsync(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book is null)
                return GenericResult<GetBookResponse>.Failure($"Book with Id = {id} is not found.");

            var response = _mapper.Map<GetBookResponse>(book);
            return GenericResult<GetBookResponse>.Success(response);
        }

        public async Task<GenericResult<GetBookResponse>> CreateAsync(CreateBookRequest request)
        {
            var author = await _authorRepository.GetByIdAsync(request.AuthorId);
            if (author is null)
                return GenericResult<GetBookResponse>.Failure($"Author with Id = {request.AuthorId} is not found.");

            var book = new Book
            {
                Title = request.Title,
                ISBN = request.ISBN,
                AuthorId = request.AuthorId,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _bookRepository.CreateAsync(book);
            var resultWithAuthor = await _bookRepository.GetByIdAsync(created.Id);
            var response = _mapper.Map<GetBookResponse>(resultWithAuthor ?? created);
            return GenericResult<GetBookResponse>.Success(response);
        }

        public async Task<GenericResult<bool>> UpdateAsync(UpdateBookRequest request)
        {
            var existing = await _bookRepository.GetByIdAsync(request.Id);
            if (existing is null)
                return GenericResult<bool>.Failure($"Book with Id = {request.Id} is not found.");

            var author = await _authorRepository.GetByIdAsync(request.AuthorId);
            if (author is null)
                return GenericResult<bool>.Failure($"Author with Id = {request.AuthorId} is not found.");

            existing.Title = request.Title;
            existing.ISBN = request.ISBN;
            existing.AuthorId = request.AuthorId;

            var updated = await _bookRepository.UpdateAsync(existing);
            return updated
                ? GenericResult<bool>.Success(true)
                : GenericResult<bool>.Failure("Failed to update book.");
        }

        public async Task<GenericResult<bool>> DeleteAsync(int id)
        {
            var isDeleted = await _bookRepository.DeleteAsync(id);
            return isDeleted
                ? GenericResult<bool>.Success(true)
                : GenericResult<bool>.Failure($"Book with Id = {id} is not found.");
        }
    }
}

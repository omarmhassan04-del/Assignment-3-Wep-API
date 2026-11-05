using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using LibrarySystemApi.Dtos.LoanDtos;
using LibrarySystemApi.Entities;
using LibrarySystemApi.Repositories;
using LibrarySystemApi.Results;

namespace LibrarySystemApi.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IBorrowerRepository _borrowerRepository;
        private readonly IMapper _mapper;

        public LoanService(
            ILoanRepository loanRepository,
            IBookRepository bookRepository,
            IBorrowerRepository borrowerRepository,
            IMapper mapper)
        {
            _loanRepository = loanRepository;
            _bookRepository = bookRepository;
            _borrowerRepository = borrowerRepository;
            _mapper = mapper;
        }

        public async Task<GenericResult<IEnumerable<GetLoanResponse>>> GetAllAsync()
        {
            var loans = await _loanRepository.GetAllAsync();
            var response = _mapper.Map<List<GetLoanResponse>>(loans);
            return GenericResult<IEnumerable<GetLoanResponse>>.Success(response);
        }

        public async Task<GenericResult<PaginatedList<GetLoanResponse>>> GetAllAsync(int pageNumber, int pageSize, string searchTerm = null)
        {
            var paginatedLoans = await _loanRepository.GetAllAsync(pageNumber, pageSize, searchTerm);
            var responseList = _mapper.Map<List<GetLoanResponse>>(paginatedLoans.Data);

            var paginatedResponse = new PaginatedList<GetLoanResponse>
            {
                PageNumber = paginatedLoans.PageNumber,
                PageSize = paginatedLoans.PageSize,
                TotalRecords = paginatedLoans.TotalRecords,
                TotalPages = paginatedLoans.TotalPages,
                Data = responseList
            };

            return GenericResult<PaginatedList<GetLoanResponse>>.Success(paginatedResponse);
        }

        public async Task<GenericResult<GetLoanResponse>> GetByIdAsync(int bookId, int borrowerId, DateTime loanDate)
        {
            var loan = await _loanRepository.GetByIdAsync(bookId, borrowerId, loanDate);
            if (loan is null)
                return GenericResult<GetLoanResponse>.Failure("Loan record is not found.");

            var response = _mapper.Map<GetLoanResponse>(loan);
            return GenericResult<GetLoanResponse>.Success(response);
        }

        public async Task<GenericResult<GetLoanResponse>> CreateAsync(CreateLoanRequest request)
        {
            var book = await _bookRepository.GetByIdAsync(request.BookId);
            if (book is null)
                return GenericResult<GetLoanResponse>.Failure($"Book with Id = {request.BookId} is not found.");

            var borrower = await _borrowerRepository.GetByIdAsync(request.BorrowerId);
            if (borrower is null)
                return GenericResult<GetLoanResponse>.Failure($"Borrower with Id = {request.BorrowerId} is not found.");

            var existing = await _loanRepository.GetByIdAsync(request.BookId, request.BorrowerId, request.LoanDate);
            if (existing is not null)
                return GenericResult<GetLoanResponse>.Failure("A loan record with the same key already exists.");

            var loan = new Loan
            {
                BookId = request.BookId,
                BorrowerId = request.BorrowerId,
                LoanDate = request.LoanDate,
                ReturnDate = request.ReturnDate,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _loanRepository.CreateAsync(loan);
            var resultWithDetails = await _loanRepository.GetByIdAsync(created.BookId, created.BorrowerId, created.LoanDate);
            var response = _mapper.Map<GetLoanResponse>(resultWithDetails ?? created);
            return GenericResult<GetLoanResponse>.Success(response);
        }

        public async Task<GenericResult<bool>> UpdateAsync(UpdateLoanRequest request)
        {
            var loan = await _loanRepository.GetByIdAsync(request.BookId, request.BorrowerId, request.LoanDate);
            if (loan is null)
                return GenericResult<bool>.Failure("Loan record is not found.");

            loan.ReturnDate = request.ReturnDate;

            var updated = await _loanRepository.UpdateAsync(loan);
            return updated
                ? GenericResult<bool>.Success(true)
                : GenericResult<bool>.Failure("Failed to update loan record.");
        }

        public async Task<GenericResult<bool>> DeleteAsync(int bookId, int borrowerId, DateTime loanDate)
        {
            var isDeleted = await _loanRepository.DeleteAsync(bookId, borrowerId, loanDate);
            return isDeleted
                ? GenericResult<bool>.Success(true)
                : GenericResult<bool>.Failure("Loan record is not found.");
        }
    }
}

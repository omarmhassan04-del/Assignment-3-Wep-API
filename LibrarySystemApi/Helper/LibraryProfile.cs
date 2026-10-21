using AutoMapper;
using LibrarySystemApi.Dtos.AuthorDtos;
using LibrarySystemApi.Dtos.BookDtos;
using LibrarySystemApi.Dtos.BorrowerDtos;
using LibrarySystemApi.Dtos.LoanDtos;
using LibrarySystemApi.Entities;

namespace LibrarySystemApi.Helper
{
    public class LibraryProfile : Profile
    {
        public LibraryProfile()
        {
            CreateMap<Author, GetAuthorResponse>()
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
                .ForMember(dest => dest.Books, opt => opt.MapFrom(src => src.Books));

            CreateMap<Book, GetBookResponse>()
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.ISBN, opt => opt.MapFrom(src => src.ISBN))
                .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author != null ? src.Author.Name : null));

            CreateMap<Borrower, GetBorrowerResponse>()
                .ForMember(dest => dest.BorrowerId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.BorrowerName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.MembershipDate, opt => opt.MapFrom(src => src.MembershipDate))
                .ForMember(dest => dest.Loans, opt => opt.MapFrom(src => src.Loans));

            CreateMap<Loan, GetLoanResponse>()
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.BookId))
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book != null ? src.Book.Title : null))
                .ForMember(dest => dest.BorrowerId, opt => opt.MapFrom(src => src.BorrowerId))
                .ForMember(dest => dest.BorrowerName, opt => opt.MapFrom(src => src.Borrower != null ? src.Borrower.Name : null))
                .ForMember(dest => dest.LoanDate, opt => opt.MapFrom(src => src.LoanDate))
                .ForMember(dest => dest.ReturnDate, opt => opt.MapFrom(src => src.ReturnDate));
        }
    }
}

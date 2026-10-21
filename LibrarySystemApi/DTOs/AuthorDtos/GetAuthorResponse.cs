using System;
using System.Collections.Generic;
using LibrarySystemApi.Dtos.BookDtos;

namespace LibrarySystemApi.Dtos.AuthorDtos
{
    public class GetAuthorResponse
    {
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public DateTime BirthDate { get; set; }
        public List<GetBookResponse> Books { get; set; } = new List<GetBookResponse>();
    }
}

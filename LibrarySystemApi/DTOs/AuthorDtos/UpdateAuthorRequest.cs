using System;

namespace LibrarySystemApi.Dtos.AuthorDtos
{
    public class UpdateAuthorRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}

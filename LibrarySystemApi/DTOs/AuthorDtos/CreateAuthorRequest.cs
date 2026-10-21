using System;

namespace LibrarySystemApi.Dtos.AuthorDtos
{
    public class CreateAuthorRequest
    {
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
    }
}

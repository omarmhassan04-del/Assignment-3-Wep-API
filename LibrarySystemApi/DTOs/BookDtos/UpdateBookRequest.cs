namespace LibrarySystemApi.Dtos.BookDtos
{
    public class UpdateBookRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public int AuthorId { get; set; }
    }
}

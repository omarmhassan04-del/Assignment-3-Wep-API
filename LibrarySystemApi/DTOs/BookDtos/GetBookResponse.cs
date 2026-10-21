namespace LibrarySystemApi.Dtos.BookDtos
{
    public class GetBookResponse
    {
        public int BookId { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
    }
}

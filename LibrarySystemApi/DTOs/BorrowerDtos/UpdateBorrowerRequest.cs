using System;

namespace LibrarySystemApi.Dtos.BorrowerDtos
{
    public class UpdateBorrowerRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime MembershipDate { get; set; }
    }
}

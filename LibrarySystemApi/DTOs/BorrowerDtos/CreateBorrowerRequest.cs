using System;

namespace LibrarySystemApi.Dtos.BorrowerDtos
{
    public class CreateBorrowerRequest
    {
        public string Name { get; set; }
        public DateTime MembershipDate { get; set; }
    }
}

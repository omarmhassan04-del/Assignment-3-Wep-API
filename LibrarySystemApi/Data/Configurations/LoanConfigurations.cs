using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LibrarySystemApi.Entities;

namespace LibrarySystemApi.Data.Configurations
{
    public class LoanConfigurations : IEntityTypeConfiguration<Loan>
    {
        public void Configure(EntityTypeBuilder<Loan> builder)
        {
            builder.HasKey(l => new { l.BookId, l.BorrowerId, l.LoanDate });

            builder.HasOne(l => l.Book)
                   .WithMany(b => b.Loans)
                   .HasForeignKey(l => l.BookId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(l => l.Borrower)
                   .WithMany(b => b.Loans)
                   .HasForeignKey(l => l.BorrowerId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(l => !l.IsDeleted);
        }
    }
}

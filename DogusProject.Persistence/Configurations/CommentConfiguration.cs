using DogusProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DogusProject.Persistence.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
	public void Configure(EntityTypeBuilder<Comment> builder)
	{
		builder.HasKey(c => c.Id);
		builder.Property(c => c.Content).IsRequired().HasMaxLength(1000);
		builder.Property(c => c.CreatedAt).IsRequired();

		builder.HasOne(c => c.Blog)
			.WithMany(b => b.Comments)
			.HasForeignKey(c => c.BlogId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.ToTable("Comments");
	}
}
using DogusProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DogusProject.Persistence.Configurations;

public class BlogImageConfiguration : IEntityTypeConfiguration<BlogImage>
{
	public void Configure(EntityTypeBuilder<BlogImage> builder)
	{
		builder.HasKey(bi => bi.Id);

		builder.Property(bi => bi.ImagePath)
			.IsRequired()
			.HasMaxLength(500);

		builder.Property(bi => bi.ImageUrl)
			.IsRequired()
			.HasMaxLength(500);

		builder.HasOne(bi => bi.Blog)
			.WithMany(b => b.BlogImages)
			.HasForeignKey(bi => bi.BlogId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
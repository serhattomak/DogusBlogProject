using DogusProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DogusProject.Persistence.Configurations;

public class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
	public void Configure(EntityTypeBuilder<Category> builder)
	{
		builder.HasKey(c => c.Id);
		builder.Property(c => c.Name).IsRequired().HasMaxLength(100);

		builder.HasMany(c => c.Blogs)
			.WithOne(b => b.Category)
			.HasForeignKey(b => b.CategoryId);

		builder.ToTable("Categories");
	}
}
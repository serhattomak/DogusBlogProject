﻿using DogusProject.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DogusProject.Persistence.Configurations;

public class BlogConfiguration : IEntityTypeConfiguration<Blog>
{
	public void Configure(EntityTypeBuilder<Blog> builder)
	{
		builder.HasKey(b => b.Id);
		builder.Property(b => b.Title).IsRequired().HasMaxLength(200);
		builder.Property(b => b.Content).IsRequired();
		builder.Property(b => b.ImagePath).HasMaxLength(500);
		builder.Property(b => b.Status).IsRequired();

		builder.HasOne(b => b.Category)
			.WithMany(c => c.Blogs)
			.HasForeignKey(b => b.CategoryId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasMany(b => b.Comments)
			.WithOne(c => c.Blog)
			.HasForeignKey(c => c.BlogId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasMany(b => b.BlogImages)
			.WithOne(bi => bi.Blog)
			.HasForeignKey(bi => bi.BlogId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.HasMany(b => b.BlogTags)
			.WithOne(bt => bt.Blog)
			.HasForeignKey(bt => bt.BlogId)
			.OnDelete(DeleteBehavior.Cascade);

		builder.ToTable("Blogs");
	}
}
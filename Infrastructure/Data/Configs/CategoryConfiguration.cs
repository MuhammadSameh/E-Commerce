using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data.Configs
{
    class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.HasKey(c => c.CategoryId);
            builder.Property(c => c.Name).HasColumnType("nvarchar(50)").IsRequired();
            builder.Property(c => c.CategoryId).IsRequired();
            builder.HasOne(category => category.ParentCategory).WithMany()
                .HasForeignKey(c => c.ParentId);
            builder.Property(c => c.ParentId).IsRequired(false);
            builder.HasIndex(c => c.Name);
        }
    }
}

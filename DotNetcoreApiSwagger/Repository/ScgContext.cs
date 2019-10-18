using DotNetcoreApiSwagger.Model.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetcoreApiSwagger.Repository
{
    public class ScgContext : DbContext, IScgContext
    {
        public ScgContext(DbContextOptions<ScgContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply configurations for entity

            modelBuilder
                .ApplyConfiguration(new RestaurantsConfiguration());

            base.OnModelCreating(modelBuilder);
        }

        public class RestaurantsConfiguration : IEntityTypeConfiguration<Restaurants>
        {
            public void Configure(EntityTypeBuilder<Restaurants> builder)
            {
                // Set configuration for table
                builder.ToTable("Restaurants", "dbo");

                // Set key for entity
                builder.HasKey(p => p.Id);

                builder.Property(p => p.Id).HasColumnType("int").IsRequired();
                builder.Property(p => p.Name).HasColumnType("nvarchar(50)").IsRequired();
                builder.Property(p => p.Address).HasColumnType("nvarchar(MAX)").IsRequired();
                builder.Property(p => p.Pricelevel).HasColumnType("int");
                builder.Property(p => p.Rating).HasColumnType("numeric(18, 2)");
            }
        }

        public DbSet<Restaurants> RestaurantEntity { get; set; }
    }
}

using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Configuration
{
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            // Configure the composite foreign key for AddressNavigation
            builder.HasOne(c => c.AddressNavigation)
                   .WithMany()  // Adjust this if there's a one-to-one or one-to-many relationship
                   .HasForeignKey(c => new { c.AddressId })  // Foreign key in Company
                   .HasPrincipalKey(a => new { a.Id });  // Principal key in Address
        }
    }
}

using Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.Configurations
{
  public class ConvenioConfiguration : IEntityTypeConfiguration<Convenio>
  {
    public void Configure(EntityTypeBuilder<Convenio> builder)
    {
      builder.HasIndex(c => c.NumeroConvenio).IsUnique();

      builder.Property(c => c.NumeroConvenio).IsRequired();
      builder.Property(c => c.Titulo).IsRequired();
      builder.Property(c => c.OrgaoConcedente).IsRequired();

      builder.HasOne(c => c.CreatedByUser)
             .WithMany()
             .HasForeignKey(c => c.CreatedByUserId)
             .OnDelete(DeleteBehavior.Restrict);
    }
  }
}

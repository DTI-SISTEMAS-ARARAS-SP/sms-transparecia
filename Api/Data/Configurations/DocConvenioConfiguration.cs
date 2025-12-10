using Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Api.Data.Configurations
{
  public class DocConvenioConfiguration : IEntityTypeConfiguration<DocConvenio>
  {
    public void Configure(EntityTypeBuilder<DocConvenio> builder)
    {
      builder.Property(d => d.TipoDocumento).IsRequired();
      builder.Property(d => d.NomeArquivoOriginal).IsRequired();
      builder.Property(d => d.NomeArquivoSalvo).IsRequired();
      builder.Property(d => d.CaminhoArquivo).IsRequired();

      builder.HasOne(d => d.Convenio)
             .WithMany()
             .HasForeignKey(d => d.ConvenioId)
             .OnDelete(DeleteBehavior.Restrict);

      builder.HasOne(d => d.UploadedByUser)
             .WithMany()
             .HasForeignKey(d => d.UploadedByUserId)
             .OnDelete(DeleteBehavior.Restrict);
    }
  }
}
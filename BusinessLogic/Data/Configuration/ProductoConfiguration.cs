using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Data.Configuration
{
    public class ProductoConfiguration : IEntityTypeConfiguration<Producto>
    {
        public void Configure(EntityTypeBuilder<Producto> builder)
        {
            builder.Property(x => x.Nombre)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(x => x.Descripcion)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Imagen)
                .HasMaxLength(1000);

            builder.Property(x => x.Precio)
                .HasColumnType("decimal(18,2)");

            // Relaciones
            builder.HasOne(x => x.Marca)
                .WithMany()
                .HasForeignKey(x => x.MarcaId);

            builder.HasOne(x => x.Categoria)
                .WithMany()
                .HasForeignKey(x => x.CategoriaId);
        }
    }
}

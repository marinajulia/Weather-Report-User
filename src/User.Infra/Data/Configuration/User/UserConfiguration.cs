using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using User.Domain.Service.User.Entities;

namespace User.Infra.Data.Configuration.User
{
    public class UserConfiguration : IEntityTypeConfiguration<UserEntity>
    {
        public void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.ToTable("User");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Email).IsRequired();
            builder.Property(p => p.Password).IsRequired();
            builder.Property(p => p.Name).IsRequired();
        }
    }
}

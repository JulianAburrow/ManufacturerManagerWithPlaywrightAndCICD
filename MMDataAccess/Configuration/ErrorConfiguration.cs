
namespace MMDataAccess.Configuration;

public class ErrorConfiguration : IEntityTypeConfiguration<ErrorModel>
{
    public void Configure(EntityTypeBuilder<ErrorModel> builder)
    {
        builder.ToTable("Error");
        builder.HasKey("ErrorId");
    }
}

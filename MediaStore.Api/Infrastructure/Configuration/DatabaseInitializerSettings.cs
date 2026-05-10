namespace MediaStore.Api.Infrastructure.Configuration;

public class DatabaseInitializerSettings
{
    public bool Enabled { get; set; }
    public string FilePath { get; set; } = "Seed/products.seed.json";
}
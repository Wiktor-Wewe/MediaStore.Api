using MediaStore.Api.Domain;
using MediaStore.Api.Infrastructure.Persistence;

namespace MediaStore.Api.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
    {
        var useInMemory = config.GetValue<bool>("DatabaseSettings:UseInMemory");

        if (useInMemory)
            services.AddSingleton<IProductRepository, InMemoryProductRepository>();
        else
            // todo AddDbContext and SqlProductRepository
            throw new NotImplementedException("SQL Server implementation not added yet.");

        return services;
    }
}

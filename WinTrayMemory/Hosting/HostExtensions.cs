using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using WinTrayMemory.Data.Entities;
using WinTrayMemory.Data.Persistence;

namespace WinTrayMemory.Hosting;

/// <summary>
/// host extensions for database initialization and registration
/// </summary>
public static class HostExtensions
{
    /// <summary>
    /// initializes the database, ensures it is created and adds default AppSettings if none exist
    /// </summary>
    public static async Task InitializeDatabaseAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<WinTrayMemoryDbContext>();

        await db.Database.EnsureCreatedAsync();

        if (!await db.AppSettings.AnyAsync())
        {
            db.AppSettings.Add(new AppSettings { Id = Guid.NewGuid() });
            await db.SaveChangesAsync();
        }
    }

    /// <summary>
    /// registers sqlite database context with path in local application data folder
    /// </summary>
    public static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        var dbPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "WinTrayMemory",
            "WinTrayMemory.db"
        );

        Directory.CreateDirectory(Path.GetDirectoryName(dbPath)!);

        services.AddDbContext<WinTrayMemoryDbContext>(options =>
            options.UseSqlite($"Data Source={dbPath}"));

        return services;
    }
}
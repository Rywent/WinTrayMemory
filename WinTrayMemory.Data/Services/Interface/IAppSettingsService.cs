using WinTrayMemory.Data.Entities;
using WinTrayMemory.Data.Persistence.DTOs.Requests;

namespace WinTrayMemory.Data.Services.Interface;

public interface IAppSettingsService
{
    Task<AppSettings> GetSettingsAsync(CancellationToken ct = default);
    Task UpdateSettingsAsync(AppSettingsRequest request, CancellationToken ct = default);
    Task<AppSettings> SaveSettingsAsync(AppSettings settings, CancellationToken ct = default);
    Task UpdateSettingsFullAsync(AppSettings settings, CancellationToken ct = default);
}

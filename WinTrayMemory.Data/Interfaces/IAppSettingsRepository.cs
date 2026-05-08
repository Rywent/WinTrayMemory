using WinTrayMemory.Data.Persistence.DTOs.Requests;
using WinTrayMemory.Data.Entities;

namespace WinTrayMemory.Data.Interfaces;

public interface IAppSettingsRepository
{
    Task<AppSettings?> GetAsync(CancellationToken cancellationToken = default);
    Task<AppSettings> UpsertAsync(AppSettings settings, CancellationToken cancellationToken = default);
    Task UpdateAsync(AppSettings settings, CancellationToken cancellationToken = default);
    Task UpdatePartialAsync(AppSettingsRequest request, CancellationToken cancellationToken = default);
}

using WinTrayMemory.Data.Entities;
using WinTrayMemory.Data.Interfaces;
using WinTrayMemory.Data.Persistence.DTOs.Requests;
using WinTrayMemory.Data.Services.Interface;
using WinTrayMemory.Settings;

namespace WinTrayMemory.Data.Services;

public class AppSettingsService : IAppSettingsService
{
    private readonly IAppSettingsRepository _repository;
    private readonly SettingsChangedEvent _settingsChanged;

    public AppSettingsService(IAppSettingsRepository repository, SettingsChangedEvent settingsChanged)
    {
        _repository = repository;
        _settingsChanged = settingsChanged;
    }

    
    public async Task<AppSettings> GetSettingsAsync(CancellationToken ct = default)
        => await _repository.GetAsync(ct) ?? throw new InvalidOperationException("Settings not found");

    public async Task UpdateSettingsAsync(AppSettingsRequest request, CancellationToken ct = default)
    {
        await _repository.UpdatePartialAsync(request, ct);
        await _settingsChanged.NotifyAsync();
    }

    public async Task<AppSettings> SaveSettingsAsync(AppSettings settings, CancellationToken ct = default)
        => await _repository.UpsertAsync(settings, ct);

    public async Task UpdateSettingsFullAsync(AppSettings settings, CancellationToken ct = default)
    {
        var current = await _repository.GetAsync(ct) ?? throw new InvalidOperationException("Settings not found");
        settings.Id = current.Id;
        await _repository.UpdateAsync(settings, ct);
    }
}

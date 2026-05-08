using Microsoft.EntityFrameworkCore;
using WinTrayMemory.Data.Entities;
using WinTrayMemory.Data.Interfaces;
using WinTrayMemory.Data.Persistence.DTOs.Requests;

namespace WinTrayMemory.Data.Persistence.Repositories;

public class AppSettingsRepository : IAppSettingsRepository
{
    private readonly WinTrayMemoryDbContext _context;

    public AppSettingsRepository(WinTrayMemoryDbContext context) => _context = context;

    public async Task<AppSettings?> GetAsync(CancellationToken ct = default)
        => await _context.AppSettings.FirstOrDefaultAsync(ct);

    public async Task<AppSettings> UpsertAsync(AppSettings settings, CancellationToken ct = default)
    {
        var existing = await GetAsync(ct);
        if (existing == null)
        {
            settings.Id = Guid.Empty;
            _context.AppSettings.Add(settings);
        }
        else
        {
            _context.Entry(existing).CurrentValues.SetValues(settings);
        }
        await _context.SaveChangesAsync(ct);
        return settings;
    }

    public async Task UpdateAsync(AppSettings settings, CancellationToken ct = default)
    {
        var entity = await GetAsync(ct) ?? throw new Exception("Not found");
        _context.Entry(entity).CurrentValues.SetValues(settings);
        await _context.SaveChangesAsync(ct);
    }
    public async Task UpdatePartialAsync(AppSettingsRequest request, CancellationToken ct = default)
    {
        var entity = await GetAsync(ct) ?? throw new InvalidOperationException("AppSettings not found");
        var entry = _context.Entry(entity);

        if (request.IsAutoCleanEnabled.HasValue)
        {
            entry.Property(e => e.IsAutoCleanEnabled).CurrentValue = request.IsAutoCleanEnabled.Value;
            entry.Property(e => e.IsAutoCleanEnabled).IsModified = true;
        }
        if (request.Threshold.HasValue)
        {
            entry.Property(e => e.Threshold).CurrentValue = request.Threshold.Value;
            entry.Property(e => e.Threshold).IsModified = true;
        }
        if (request.ShowNotification.HasValue)
        {
            entry.Property(e => e.ShowNotification).CurrentValue = request.ShowNotification.Value;
            entry.Property(e => e.ShowNotification).IsModified = true;
        }
        if (request.AllowKillSystemProcesses.HasValue)
        {
            entry.Property(e => e.AllowKillSystemProcesses).CurrentValue = request.AllowKillSystemProcesses.Value;
            entry.Property(e => e.AllowKillSystemProcesses).IsModified = true;
        }
        if (request.IsRunOnStartup.HasValue)
        {
            entry.Property(e => e.IsRunOnStartup).CurrentValue = request.IsRunOnStartup.Value;
            entry.Property(e => e.IsRunOnStartup).IsModified = true;
        }
        if (request.IsCheckForUpdates.HasValue)
        {
            entry.Property(e => e.IsCheckForUpdates).CurrentValue = request.IsCheckForUpdates.Value;
            entry.Property(e => e.IsCheckForUpdates).IsModified = true;
        }
        if (request.MinProcessSize.HasValue)
        {
            entry.Property(e => e.MinProcessSize).CurrentValue = request.MinProcessSize.Value;
            entry.Property(e => e.MinProcessSize).IsModified = true;
        }
        if (request.MaxProcessesShown.HasValue)
        {
            entry.Property(e => e.MaxProcessesShown).CurrentValue = request.MaxProcessesShown.Value;
            entry.Property(e => e.MaxProcessesShown).IsModified = true;
        }
        if (request.RefreshInterval.HasValue)
        {
            entry.Property(e => e.RefreshInterval).CurrentValue = request.RefreshInterval.Value;
            entry.Property(e => e.RefreshInterval).IsModified = true;
        }
        if (request.CleanWorkingSet.HasValue)
        {
            entry.Property(e => e.CleanWorkingSet).CurrentValue = request.CleanWorkingSet.Value;
            entry.Property(e => e.CleanWorkingSet).IsModified = true;
        }
        if (request.CleanLowPriorityStandby.HasValue)
        {
            entry.Property(e => e.CleanLowPriorityStandby).CurrentValue = request.CleanLowPriorityStandby.Value;
            entry.Property(e => e.CleanLowPriorityStandby).IsModified = true;
        }
        if (request.CleanStandbyList.HasValue)
        {
            entry.Property(e => e.CleanStandbyList).CurrentValue = request.CleanStandbyList.Value;
            entry.Property(e => e.CleanStandbyList).IsModified = true;
        }
        if (request.CleanModifiedPageList.HasValue)
        {
            entry.Property(e => e.CleanModifiedPageList).CurrentValue = request.CleanModifiedPageList.Value;
            entry.Property(e => e.CleanModifiedPageList).IsModified = true;
        }

        if (entry.Properties.Any(p => p.IsModified))
        {
            await _context.SaveChangesAsync(ct);
        }
    }
}

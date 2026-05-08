using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinTrayMemory.Data.Entities;
using WinTrayMemory.Data.Interfaces;

namespace WinTrayMemory.Data.Persistence.Repositories;

public class ProcessRuleRepository : IProcessRuleRepository
{
    private readonly WinTrayMemoryDbContext _context;

    public ProcessRuleRepository(WinTrayMemoryDbContext context) => _context = context;

    public async Task<IEnumerable<ProcessRule>> GetAllAsync(CancellationToken ct = default)
        => await _context.ProcessRules.ToListAsync(ct);

    public async Task<ProcessRule?> GetByIdAsync(Guid id, CancellationToken ct = default)
       => await _context.ProcessRules.FindAsync(new object?[] { id }, cancellationToken: ct);

    public async Task<ProcessRule> AddAsync(ProcessRule rule, CancellationToken ct = default)
    {
        _context.ProcessRules.Add(rule);
        await _context.SaveChangesAsync(ct);
        return rule;
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var rule = await GetByIdAsync(id, ct) ?? throw new Exception("Not found");
        _context.ProcessRules.Remove(rule);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<bool> ExistsByNameAsync(string name, CancellationToken ct = default)
        => await _context.ProcessRules.AnyAsync(r => r.Name == name, ct);
}

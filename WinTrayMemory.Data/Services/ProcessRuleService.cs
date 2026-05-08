using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinTrayMemory.Data.Entities;
using WinTrayMemory.Data.Interfaces;
using WinTrayMemory.Data.Services.Interface;

namespace WinTrayMemory.Data.Services;

public class ProcessRuleService : IProcessRuleService
{
    private readonly IProcessRuleRepository _repository;

    public ProcessRuleService(IProcessRuleRepository repository)
        => _repository = repository;

    public async Task<IEnumerable<ProcessRule>> GetAllRulesAsync(CancellationToken ct = default)
        => await _repository.GetAllAsync(ct);

    public async Task<ProcessRule> AddRuleAsync(ProcessRule rule, CancellationToken ct = default)
    {
        if (await _repository.ExistsByNameAsync(rule.Name, ct))
            throw new InvalidOperationException($"Rule '{rule.Name}' already exists");
        return await _repository.AddAsync(rule, ct);
    }

    public async Task DeleteRuleAsync(Guid id, CancellationToken ct = default)
        => await _repository.DeleteAsync(id, ct);

    public Task<bool> RuleExistsAsync(string name, CancellationToken ct = default)
        => _repository.ExistsByNameAsync(name, ct);

}
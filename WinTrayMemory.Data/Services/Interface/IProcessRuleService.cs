using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinTrayMemory.Data.Entities;

namespace WinTrayMemory.Data.Services.Interface;

public interface IProcessRuleService
{
    Task<IEnumerable<ProcessRule>> GetAllRulesAsync(CancellationToken ct = default);
    Task<ProcessRule> AddRuleAsync(ProcessRule rule, CancellationToken ct = default);
    Task DeleteRuleAsync(Guid id, CancellationToken ct = default);
    Task<bool> RuleExistsAsync(string name, CancellationToken ct = default);
}

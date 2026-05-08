using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinTrayMemory.Data.Entities;

namespace WinTrayMemory.Data.Interfaces;

public interface IProcessRuleRepository
{
    Task<IEnumerable<ProcessRule>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ProcessRule?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ProcessRule> AddAsync(ProcessRule rule, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken = default);
}

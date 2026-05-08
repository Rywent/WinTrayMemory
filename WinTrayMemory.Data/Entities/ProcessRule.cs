using System.ComponentModel.DataAnnotations;
using WinTrayMemory.Data.Enums;

namespace WinTrayMemory.Data.Entities;

public class ProcessRule
{
    [Key]
    public Guid Id { get; set; }
    [MaxLength(256)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(50)]
    public Category Category { get; set; } = Category.Safe;
    [MaxLength(100)]
    public string ClueMessage { get; set; } = string.Empty;
}


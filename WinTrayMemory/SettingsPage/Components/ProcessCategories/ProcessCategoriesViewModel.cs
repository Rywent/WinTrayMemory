using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WinTrayMemory.Data.Entities;
using WinTrayMemory.Data.Enums;
using WinTrayMemory.Data.Services.Interface;
using WinTrayMemory.Processes;
using static WinTrayMemory.Processes.DeterminingProcessType;

namespace WinTrayMemory.SettingsPage.Components.ProcessCategories;

public partial class ProcessCategoriesViewModel : ObservableObject
{
    private readonly IProcessRuleService _processRuleService;

    [ObservableProperty]
    private ObservableCollection<UserProcessInfo> _processes = new();

    [ObservableProperty]
    private string _enteredProcessName = string.Empty;

    [ObservableProperty]
    private string _selectedCategory = "✓ Safe";

    public ObservableCollection<string> Categories { get; } = new()
    {
        "✓ Safe",
        "⚠ Warning",
        "✕ System"
    };

    public ProcessCategoriesViewModel(IProcessRuleService processRuleService)
    {
        _processRuleService = processRuleService;
    }
    public async Task LoadRulesAsync()
    {
        try
        {
            var rules = await _processRuleService.GetAllRulesAsync();

            Processes.Clear();
            foreach (var rule in rules)
            {
                var processType = rule.Category switch
                {
                    Category.Safe => ProcessType.Safely,
                    Category.Warning => ProcessType.Warning,
                    Category.Dangerous => ProcessType.Dangerous,
                    _ => ProcessType.Safely
                };

                Processes.Add(new UserProcessInfo(
                    rule.Name,
                    rule.ClueMessage,
                    processType
                ));
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Load rules error: {ex.Message}");
        }
    }

    /// <summary>
    /// adds a new user process rule to the list with category-based clue message
    /// </summary>

    [RelayCommand]
    private async Task AddRule()
    {
        if (string.IsNullOrWhiteSpace(EnteredProcessName))
            return;

        var processName = EnteredProcessName.Trim().ToLower().Replace(".exe", "");

        if (Processes.Any(p => p.Name.Equals(processName, StringComparison.OrdinalIgnoreCase)))
        {
            MessageBox.Show($"Rule '{processName}' already exists in list");
            EnteredProcessName = string.Empty;
            return;
        }

        var category = SelectedCategory switch
        {
            "✓ Safe" => ProcessType.Safely,
            "⚠ Warning" => ProcessType.Warning,
            "✕ System" => ProcessType.Dangerous,
            _ => ProcessType.Safely
        };

        var clueMessage = GetClueMessage(category);

        try
        {
            var rule = new ProcessRule
            {
                Id = Guid.NewGuid(),
                Name = processName,
                Category = ConvertToCategoryEnum(category),
                ClueMessage = clueMessage
            };

            await _processRuleService.AddRuleAsync(rule);

            Processes.Add(new UserProcessInfo(processName, clueMessage, category));
            EnteredProcessName = string.Empty;
        }
        catch (InvalidOperationException ex)
        {
            MessageBox.Show($"Add rule error: {ex.Message}");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Add rule error: {ex.Message}");
        }
    }

    /// <summary>
    /// removes the selected process rule from the list and database
    /// </summary>
    [RelayCommand]
    private async Task RemoveProcess(UserProcessInfo? process)
    {
        if (process is null)
            return;

        try
        {
            var rules = await _processRuleService.GetAllRulesAsync();
            var ruleToDelete = rules.FirstOrDefault(r =>
                r.Name.Equals(process.Name, StringComparison.OrdinalIgnoreCase));

            if (ruleToDelete != null)
            {
                await _processRuleService.DeleteRuleAsync(ruleToDelete.Id);
            }

            Processes.Remove(process);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Remove rule error: {ex.Message}");
        }
    }

    private string GetClueMessage(ProcessType type)
    {
        return type switch
        {
            ProcessType.Safely => "Safe process - can be killed without risk",
            ProcessType.Warning => "Warning process - kill with caution",
            ProcessType.Dangerous => "Critical system process - cannot kill",
            _ => "Unknown process type"
        };
    }

    private Category ConvertToCategoryEnum(ProcessType type)
    {
        return type switch
        {
            ProcessType.Safely => Category.Safe,
            ProcessType.Warning => Category.Warning,
            ProcessType.Dangerous => Category.Dangerous,
            _ => Category.Safe
        };
    }
}

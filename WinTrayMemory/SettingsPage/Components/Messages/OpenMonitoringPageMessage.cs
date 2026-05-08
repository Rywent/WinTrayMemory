using CommunityToolkit.Mvvm.Messaging.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinTrayMemory.SettingsPage.Components.Messages;

public sealed class OpenMonitoringPageMessage : ValueChangedMessage<bool>
{
    public OpenMonitoringPageMessage(bool isOpen) : base(isOpen) { }
}
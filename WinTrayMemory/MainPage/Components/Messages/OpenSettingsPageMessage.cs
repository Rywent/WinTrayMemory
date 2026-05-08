using CommunityToolkit.Mvvm.Messaging.Messages;

namespace WinTrayMemory.MainPage.Components.Messages;

public sealed class OpenSettingsPageMessage : ValueChangedMessage<bool>
{
    public OpenSettingsPageMessage(bool isOpen) : base(isOpen) { }
}

using TabBlazor;

namespace DaaS.UI.Blazor.Services;

public class AppSettings
{
    public bool DarkMode { get; set; } = true;
    public NavbarDirection NavbarDirection { get; set; } = NavbarDirection.Horizontal;
    public NavbarBackground NavbarBackground { get; set; } = NavbarBackground.Dark;

}

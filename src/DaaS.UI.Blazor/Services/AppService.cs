﻿namespace DaaS.UI.Blazor.Services;

public class AppService
{
    private AppSettings settings = new AppSettings();

    public AppSettings Settings => settings;
    public Action OnSettingsUpdated;

    public void SettingsUpdated()
    {
        OnSettingsUpdated.Invoke();
    }
}


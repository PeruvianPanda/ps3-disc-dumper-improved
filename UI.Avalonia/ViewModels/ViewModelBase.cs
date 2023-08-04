﻿using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using UI.Avalonia.Utils;

namespace UI.Avalonia.ViewModels;

public partial class ViewModelBase: ObservableObject
{
    [ObservableProperty] private static string tintColor = "#ffffff";
    [ObservableProperty] private static double tintOpacity = 1.0;
    [ObservableProperty] private static double materialOpacity = 0.69;
    [ObservableProperty] private static double luminosityOpacity = 1.0;
    [ObservableProperty] private static string accentColor = ThemeConsts.AccentColor;
    [ObservableProperty] private static bool micaEnabled = true;
    [ObservableProperty] private static bool acrylicEnabled = false;
    [ObservableProperty] private static string dimTextColor = "#ff0000"; //ThemeConsts.LightThemeDimGray;
    [ObservableProperty] private static string hoverLayerColor = "#ff0000"; //ThemeConsts.LightThemeLayerHover;
    [NotifyPropertyChangedFor(nameof(SettingsSymbol))]
    [ObservableProperty] private static FontFamily symbolFontFamily = new("avares://ps3-disc-dumper/Assets/Fonts#Font Awesome 6 Free");
    [ObservableProperty] private static FontFamily largeFontFamily = FontManager.Current.DefaultFontFamily;
    [ObservableProperty] private static FontFamily smallFontFamily = FontManager.Current.DefaultFontFamily;
    
    public string SettingsSymbol => SymbolFontFamily.Name is "Segoe Fluent Icons" ? "\ue713" : "\uf013"; 
    public string BackSymbol => SymbolFontFamily.Name is "Segoe Fluent Icons" ? "\ue72b" : "\uf060"; 
    
    [ObservableProperty] protected string pageTitle = "PS3 Disc Dumper";
    [ObservableProperty] private bool canEditSettings = true;
}
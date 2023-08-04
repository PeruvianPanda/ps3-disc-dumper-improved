using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;
using UI.Avalonia.Utils;
using UI.Avalonia.ViewModels;
using UI.Avalonia.Views;

namespace UI.Avalonia;

public partial class App : Application
{
    private static readonly WindowTransparencyLevel[] DesiredTransparencyHints =
    {
        WindowTransparencyLevel.Mica,
        WindowTransparencyLevel.AcrylicBlur,
        WindowTransparencyLevel.None,
    };

    private readonly Lazy<bool> isMicaCapable = new(() =>
        Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime { MainWindow: Window w }
        && w.ActualTransparencyLevel == WindowTransparencyLevel.Mica
    );

    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };
            desktop.MainWindow.Activated += OnActivated;
            desktop.MainWindow.Deactivated += OnDeactivated;
            desktop.MainWindow.ActualThemeVariantChanged += OnThemeChanged;

            if (desktop is { MainWindow: { DataContext: MainWindowViewModel vm } w})
            {
                vm.CurrentPage.MicaEnabled = isMicaCapable.Value;
                vm.CurrentPage.AcrylicEnabled = w.ActualTransparencyLevel == WindowTransparencyLevel.AcrylicBlur;
            }
            /*
            if (isMicaCapable.Value && desktop.MainWindow is Window w)
                RenderOptions.SetTextRenderingMode(w, TextRenderingMode.Antialias);
            */
        }
        base.OnFrameworkInitializationCompleted();
    }

    private void OnActivated(object? sender, EventArgs e)
    {
        if (sender is not Window w)
            return;

        if (isMicaCapable.Value)
            w.TransparencyLevelHint = DesiredTransparencyHints;
    }

    private void OnDeactivated(object? sender, EventArgs e)
    {
        if (sender is not Window { DataContext: MainWindowViewModel vm } w)
            return;

        if (isMicaCapable.Value)
            w.TransparencyLevelHint = Array.Empty<WindowTransparencyLevel>();
        if (w.ActualThemeVariant == ThemeVariant.Light)
            vm.CurrentPage.TintColor = ThemeConsts.LightThemeTintColor;
        else if (w.ActualThemeVariant == ThemeVariant.Dark)
            vm.CurrentPage.TintColor = ThemeConsts.DarkThemeTintColor;
    }

    internal static void OnThemeChanged(object? sender, EventArgs e)
    {
        if (sender is not Window { DataContext: MainWindowViewModel {CurrentPage: ViewModelBase cpvm} vm } window)
            return;

        if (window.ActualThemeVariant == ThemeVariant.Light)
        {
            vm.CurrentPage.TintColor = ThemeConsts.LightThemeTintColor;
            vm.CurrentPage.DimTextColor = ThemeConsts.LightThemeDimGray;
            vm.CurrentPage.HoverLayerColor = ThemeConsts.LightThemeLayerHover;
        }
        else if (window.ActualThemeVariant == ThemeVariant.Dark)
        {
            vm.CurrentPage.TintColor = ThemeConsts.DarkThemeTintColor;
            vm.CurrentPage.DimTextColor = ThemeConsts.DarkThemeDimGray;
            vm.CurrentPage.HoverLayerColor = ThemeConsts.DarkThemeLayerHover;
        }
    }
}
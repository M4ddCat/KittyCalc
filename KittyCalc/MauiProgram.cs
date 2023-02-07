using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;

namespace KittyCalc;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif
#if WINDOWS
		builder.ConfigureLifecycleEvents(events =>
		{
			events.AddWindows(wndLifeCycleBuilder =>
			{
				wndLifeCycleBuilder.OnWindowCreated(window =>
				{
					IntPtr nativeWindowHandle = WinRT.Interop.WindowNative.GetWindowHandle(window);
					WindowId win32WindowsId = Win32Interop.GetWindowIdFromWindow(nativeWindowHandle);
					AppWindow winuiAppWindow = AppWindow.GetFromWindowId(win32WindowsId);
					if (winuiAppWindow.Presenter is OverlappedPresenter p)
					{
						//p.IsAlwaysOnTop=true;
						p.IsResizable = true;
						p.IsMaximizable = true;
						p.IsMinimizable = true;
						const int width = 500;
						const int height = 800;
						winuiAppWindow.MoveAndResize(new RectInt32(1920 / 2 - width / 2, 1080 / 2 - height / 2, width, height));
					}
					else
					{
						const int width = 300;
						const int height = 800;
						winuiAppWindow.MoveAndResize(new RectInt32(300 / 2 - width / 2, 800 / 2 - height / 2, width, height));
					}
				});
			});
		});
#endif
		return builder.Build();
	}
}

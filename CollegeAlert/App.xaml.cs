using Microsoft.Extensions.DependencyInjection;
using CollegeAlert.Services;

namespace CollegeAlert;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
		AppLog.Info("App", "application initialized");
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		AppLog.Info("App", "creating main window");
		return new Window(new AppShell());
	}
}

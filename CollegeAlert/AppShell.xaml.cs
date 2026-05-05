namespace CollegeAlert;
using CollegeAlert.Services;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		AppLog.Info("AppShell", "created");
		Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
		AppLog.Info("AppShell", "routes registered", ("route", nameof(LoginPage)));
	}
}

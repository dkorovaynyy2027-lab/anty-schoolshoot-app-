namespace CollegeAlert;

using CollegeAlert.Services;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        AppLog.Info("LoginPage", "created");
    }

    private async void OnLoginClicked(object? sender, EventArgs e)
    {
        AppLog.Tap("LoginPage", "login button", ("login", LoginEntry.Text));

        if (LoginEntry.Text == "123" && PasswordEntry.Text == "123")
        {
            AppLog.Info("LoginPage", "login success, navigating to admin");
            await Shell.Current.GoToAsync("//AdminPage");
        }
        else
        {
            AppLog.Warn("LoginPage", "login failed", ("login", LoginEntry.Text));
            await DisplayAlertAsync("Помилка", "Невірний логін або пароль", "OK");
        }
    }

    private async void OnCancelClicked(object? sender, EventArgs e)
    {
        AppLog.Tap("LoginPage", "cancel login");
        await Shell.Current.GoToAsync("//MainPage");
    }
}

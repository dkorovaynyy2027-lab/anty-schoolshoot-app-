using System.Collections.ObjectModel;
using CollegeAlert.Services;

namespace CollegeAlert;

public partial class AdminPage : ContentPage
{
    private AlertService alertService;
    public ObservableCollection<SignalDto> IncomingSignals { get; set; } = new();

    public AdminPage()
    {
        InitializeComponent();
        SignalsList.ItemsSource = IncomingSignals;
        
        alertService = new AlertService();

        alertService.PendingSignalReceived += (signal) =>
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                IncomingSignals.Insert(0, signal);
                
                // Автоматично перемикаємо на вкладку сигналів, щоб адмін одразу бачив SOS
                OnTabAlertsClicked(this, EventArgs.Empty);

                if (Vibration.Default.IsSupported)
                    Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(800));
            });
        };

        alertService.StatsUpdated += (stats) =>
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                SafeCountLbl.Text = stats.SafeCount.ToString();
                DangerCountLbl.Text = stats.DangerCount.ToString();
                AwayCountLbl.Text = stats.AwayCount.ToString();
                
                // Animate changes
                SafeCountLbl.ScaleTo(1.2, 100).ContinueWith(t => SafeCountLbl.ScaleTo(1.0, 100));
                DangerCountLbl.ScaleTo(1.2, 100).ContinueWith(t => DangerCountLbl.ScaleTo(1.0, 100));
                AwayCountLbl.ScaleTo(1.2, 100).ContinueWith(t => AwayCountLbl.ScaleTo(1.0, 100));
            });
        };
    }

    private void OnLoginClicked(object sender, EventArgs e)
    {
        if (LoginEntry.Text == "123" && PasswordEntry.Text == "123")
        {
            LoginView.IsVisible = false;
            AdminContentView.IsVisible = true;
            Preferences.Set("Role", "Admin");
            _ = alertService.Initialize("Admin");
        }
        else
        {
            DisplayAlert("Помилка", "Невірний логін або пароль", "OK");
        }
    }

    private void OnLogoutClicked(object sender, EventArgs e)
    {
        LoginView.IsVisible = true;
        AdminContentView.IsVisible = false;
        PasswordEntry.Text = "";
        Preferences.Set("Role", "Student");
    }

    private void OnTabStatsClicked(object sender, EventArgs e)
    {
        TabStats.BackgroundColor = Colors.White;
        TabStats.TextColor = Color.FromArgb("#1E293B");
        TabAlerts.BackgroundColor = Colors.Transparent;
        TabAlerts.TextColor = Color.FromArgb("#64748B");
        
        StatsContainer.IsVisible = true;
        AlertsContainer.IsVisible = false;
    }

    private void OnTabAlertsClicked(object sender, EventArgs e)
    {
        TabAlerts.BackgroundColor = Colors.White;
        TabAlerts.TextColor = Color.FromArgb("#1E293B");
        TabStats.BackgroundColor = Colors.Transparent;
        TabStats.TextColor = Color.FromArgb("#64748B");
        
        StatsContainer.IsVisible = false;
        AlertsContainer.IsVisible = true;
    }

    private async void OnConfirmIncident(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Підтвердження", "Оголосити загальну тривогу?", "ТАК", "НІ");
        if (confirm)
        {
            string building = "Невідомо";
            string floor = "Невідомо";
            if (IncomingSignals.Count > 0)
            {
                building = IncomingSignals[0].Building;
                floor = IncomingSignals[0].Floor;
            }
            await alertService.ConfirmAlert(Guid.NewGuid().ToString(), building, floor);
        }
    }

    private async void OnEndAlertClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Відбій", "Скасувати тривогу і повернути всіх до нормального стану?", "ТАК", "НІ");
        if (confirm)
        {
            IncomingSignals.Clear();
            await alertService.EndAlert();
            await DisplayAlert("Відбій", "Тривогу скасовано.", "OK");
        }
    }
}

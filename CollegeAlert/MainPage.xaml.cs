namespace CollegeAlert;

public partial class MainPage : ContentPage
{
    private CancellationTokenSource? holdTokenSource;
    private bool incidentActive;
    private bool adminMode;
    private int themeMode;
    private int receivedCount;
    private int helpCount;

    public MainPage()
    {
        InitializeComponent();
        UpdateRoleUi();
        UpdateIncidentUi();
    }

    private async void OnAlarmPressed(object? sender, EventArgs e)
    {
        if (incidentActive)
        {
            return;
        }

        holdTokenSource?.Cancel();
        holdTokenSource = new CancellationTokenSource();
        var token = holdTokenSource.Token;

        try
        {
            HoldProgress.Opacity = 0.2;
            await AlarmButton.ScaleToAsync(0.95, 120, Easing.CubicOut);
            Pulse(TimeSpan.FromMilliseconds(120));

            await Task.WhenAll(
                HoldProgress.FadeToAsync(1, 2600, Easing.CubicInOut),
                HoldProgress.RotateToAsync(270, 3000, Easing.Linear));

            token.ThrowIfCancellationRequested();
            ActivateIncident("Сигнал надіслано", false);
        }
        catch (OperationCanceledException)
        {
            await ResetHoldVisuals();
        }
    }

    private async void OnAlarmReleased(object? sender, EventArgs e)
    {
        if (incidentActive)
        {
            return;
        }

        holdTokenSource?.Cancel();
        await ResetHoldVisuals();
    }

    private void OnStudentRoleClicked(object? sender, EventArgs e)
    {
        adminMode = false;
        UpdateRoleUi();
    }

    private void OnAdminRoleClicked(object? sender, EventArgs e)
    {
        adminMode = true;
        UpdateRoleUi();
    }

    private void OnThemeClicked(object? sender, EventArgs e)
    {
        themeMode = (themeMode + 1) % 3;

        Application.Current!.UserAppTheme = themeMode switch
        {
            1 => AppTheme.Light,
            2 => AppTheme.Dark,
            _ => AppTheme.Unspecified
        };

        ThemeButton.Text = themeMode switch
        {
            1 => "Світла",
            2 => "Темна",
            _ => "Авто"
        };
    }

    private void OnConfirmIncidentClicked(object? sender, EventArgs e)
    {
        ActivateIncident("Підтверджено адміністрацією", false);
        receivedCount = Math.Max(receivedCount, 18);
        helpCount = Math.Max(helpCount, 2);
        UpdateCounters();
        Pulse(TimeSpan.FromMilliseconds(900));
    }

    private void OnDrillClicked(object? sender, EventArgs e)
    {
        ActivateIncident("Навчальна тривога", true);
        receivedCount = 12;
        helpCount = 0;
        UpdateCounters();
        Pulse(TimeSpan.FromMilliseconds(350));
    }

    private void OnCancelIncidentClicked(object? sender, EventArgs e)
    {
        incidentActive = false;
        receivedCount = 0;
        helpCount = 0;
        UpdateIncidentUi();
        UpdateCounters();
    }

    private async void OnSafeClicked(object? sender, EventArgs e)
    {
        receivedCount++;
        UpdateCounters();
        Pulse(TimeSpan.FromMilliseconds(180));
        await DisplayAlertAsync("Статус надіслано", "Позначено: я в безпеці.", "OK");
    }

    private async void OnNeedHelpClicked(object? sender, EventArgs e)
    {
        receivedCount++;
        helpCount++;
        UpdateCounters();
        Pulse(TimeSpan.FromMilliseconds(700));
        await DisplayAlertAsync("Статус надіслано", "Адміністрація бачить, що вам потрібна допомога.", "OK");
    }

    private async void OnAwayClicked(object? sender, EventArgs e)
    {
        receivedCount++;
        UpdateCounters();
        Pulse(TimeSpan.FromMilliseconds(140));
        await DisplayAlertAsync("Статус надіслано", "Позначено: ви не в цьому корпусі.", "OK");
    }

    private async void OnInstructionsClicked(object? sender, EventArgs e)
    {
        await DisplayAlertAsync(
            "Інструкції",
            "Залишайтеся на місці, не створюйте шуму, дійте за правилами безпеки коледжу та очікуйте подальших повідомлень.",
            "OK");
    }

    private void ActivateIncident(string caption, bool isDrill)
    {
        incidentActive = true;
        HeaderSubtitle.Text = caption;
        ModeTitle.Text = isDrill ? "Навчальна тривога" : "Активна тривога";
        ModeSubtitle.Text = isDrill ? "Тренування без реальної загрози" : "Корпус Б - 3 поверх";
        ModeBadge.Text = isDrill ? "DR" : "SOS";
        PrimaryStatusLabel.Text = isDrill ? "Тренувальне оповіщення активне" : "Тихе сповіщення активне";
        SecondaryStatusLabel.Text = "Відкрийте статус або очікуйте подальших повідомлень";
        AlarmButton.Text = isDrill ? "НАВЧАЛЬНА\nТРИВОГА" : "СИГНАЛ\nНАДІСЛАНО";
        AlarmButton.BackgroundColor = isDrill ? Color.FromArgb("#C89618") : Color.FromArgb("#B73535");
    }

    private void UpdateIncidentUi()
    {
        HeaderSubtitle.Text = "Немає активних тривог";
        ModeTitle.Text = "Режим спостереження";
        ModeSubtitle.Text = "Корпус Б - 3 поверх";
        ModeBadge.Text = "OK";
        PrimaryStatusLabel.Text = "Зараз активних тривог немає";
        SecondaryStatusLabel.Text = "Система готова до тихого екстреного сповіщення";
        AlarmButton.Text = "ТРИВОГА\nУтримуйте 3 секунди";
        AlarmButton.BackgroundColor = Color.FromArgb("#E94040");
        HoldProgress.Opacity = 0;
        HoldProgress.Rotation = -90;
    }

    private void UpdateRoleUi()
    {
        StudentPanel.IsVisible = !adminMode;
        AdminPanel.IsVisible = adminMode;

        StudentRoleButton.BackgroundColor = adminMode ? Colors.Transparent : Color.FromArgb("#2F67D8");
        StudentRoleButton.TextColor = adminMode ? Color.FromArgb("#7A8AA5") : Colors.White;
        AdminRoleButton.BackgroundColor = adminMode ? Color.FromArgb("#2F67D8") : Colors.Transparent;
        AdminRoleButton.TextColor = adminMode ? Colors.White : Color.FromArgb("#7A8AA5");

        ModeSubtitle.Text = adminMode ? "Панель підтвердження та розсилки" : "Корпус Б - 3 поверх";
    }

    private void UpdateCounters()
    {
        ReceivedCountLabel.Text = receivedCount.ToString();
        HelpCountLabel.Text = helpCount.ToString();
    }

    private async Task ResetHoldVisuals()
    {
        await Task.WhenAll(
            AlarmButton.ScaleToAsync(1, 120, Easing.CubicOut),
            HoldProgress.FadeToAsync(0, 120, Easing.CubicOut));
        HoldProgress.Rotation = -90;
    }

    private static void Pulse(TimeSpan duration)
    {
        if (Vibration.Default.IsSupported)
        {
            Vibration.Default.Vibrate(duration);
        }
    }
}

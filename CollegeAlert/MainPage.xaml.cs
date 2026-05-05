namespace CollegeAlert;

public partial class MainPage : ContentPage
{
    private CancellationTokenSource? holdTokenSource;
    private bool incidentActive;
    private bool adminMode;
    private int themeMode;
    private int receivedCount;
    private int helpCount;
    private int coverage;
    private AlertScope alertScope = AlertScope.Building;
    private AlertVibration vibrationMode = AlertVibration.Moderate;

    public MainPage()
    {
        InitializeComponent();
        UpdateRoleUi();
        UpdateIncidentUi();
        UpdateCounters();
        UpdateScopeUi();
        UpdateVibrationUi();
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
            await AlarmButton.ScaleToAsync(0.94, 120, Easing.CubicOut);
            Pulse(TimeSpan.FromMilliseconds(120));

            await Task.WhenAll(
                HoldProgress.FadeToAsync(1, 2600, Easing.CubicInOut),
                HoldProgress.RotateToAsync(270, 3000, Easing.Linear));

            token.ThrowIfCancellationRequested();
            ActivateIncident("Сигнал надіслано черговому", false);
            AddEvent("Студент надіслав тихий сигнал тривоги");
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

    private void OnScopeBuildingClicked(object? sender, EventArgs e)
    {
        alertScope = AlertScope.Building;
        UpdateScopeUi();
    }

    private void OnScopeCampusClicked(object? sender, EventArgs e)
    {
        alertScope = AlertScope.Campus;
        UpdateScopeUi();
    }

    private void OnScopeAllClicked(object? sender, EventArgs e)
    {
        alertScope = AlertScope.College;
        UpdateScopeUi();
    }

    private void OnSilentVibeClicked(object? sender, EventArgs e)
    {
        vibrationMode = AlertVibration.Silent;
        UpdateVibrationUi();
    }

    private void OnModerateVibeClicked(object? sender, EventArgs e)
    {
        vibrationMode = AlertVibration.Moderate;
        UpdateVibrationUi();
    }

    private void OnLoudVibeClicked(object? sender, EventArgs e)
    {
        vibrationMode = AlertVibration.Loud;
        UpdateVibrationUi();
    }

    private void OnConfirmIncidentClicked(object? sender, EventArgs e)
    {
        ActivateIncident("Інцидент підтверджено і розіслано", false);
        receivedCount = alertScope switch
        {
            AlertScope.Building => 42,
            AlertScope.Campus => 96,
            _ => 184
        };
        helpCount = alertScope == AlertScope.Building ? 3 : 7;
        coverage = 68;
        UpdateCounters();
        Pulse(GetVibrationDuration());
        AddEvent($"Адміністрація підтвердила інцидент: {GetScopeText()}, {GetVibrationText()}");
    }

    private void OnDrillClicked(object? sender, EventArgs e)
    {
        ActivateIncident("Навчальна тривога активна", true);
        receivedCount = 28;
        helpCount = 0;
        coverage = 35;
        UpdateCounters();
        Pulse(TimeSpan.FromMilliseconds(250));
        AddEvent($"Запущено навчальну тривогу: {GetScopeText()}");
    }

    private void OnCancelIncidentClicked(object? sender, EventArgs e)
    {
        incidentActive = false;
        receivedCount = 0;
        helpCount = 0;
        coverage = 0;
        UpdateIncidentUi();
        UpdateCounters();
        AddEvent("Тривогу скасовано відповідальною особою");
    }

    private async void OnSafeClicked(object? sender, EventArgs e)
    {
        receivedCount++;
        coverage = Math.Min(100, coverage + 8);
        UpdateCounters();
        Pulse(TimeSpan.FromMilliseconds(160));
        AddEvent("Користувач позначив: я в безпеці");
        await DisplayAlertAsync("Статус надіслано", "Позначено: я в безпеці.", "OK");
    }

    private async void OnNeedHelpClicked(object? sender, EventArgs e)
    {
        receivedCount++;
        helpCount++;
        coverage = Math.Min(100, coverage + 8);
        UpdateCounters();
        Pulse(TimeSpan.FromMilliseconds(700));
        AddEvent("Користувач позначив: потрібна допомога");
        await DisplayAlertAsync("Статус надіслано", "Адміністрація бачить, що вам потрібна допомога.", "OK");
    }

    private async void OnAwayClicked(object? sender, EventArgs e)
    {
        receivedCount++;
        coverage = Math.Min(100, coverage + 8);
        UpdateCounters();
        Pulse(TimeSpan.FromMilliseconds(130));
        AddEvent("Користувач позначив: не в цій будівлі");
        await DisplayAlertAsync("Статус надіслано", "Позначено: ви не в цьому корпусі.", "OK");
    }

    private async void OnReceivedClicked(object? sender, EventArgs e)
    {
        receivedCount++;
        coverage = Math.Min(100, coverage + 6);
        UpdateCounters();
        Pulse(TimeSpan.FromMilliseconds(120));
        AddEvent("Користувач підтвердив отримання повідомлення");
        await DisplayAlertAsync("Отримано", "Підтвердження доставлено адміністрації.", "OK");
    }

    private async void OnInstructionsClicked(object? sender, EventArgs e)
    {
        await DisplayAlertAsync(
            "Інструкції",
            "Залишайтеся на місці, дійте за правилами безпеки коледжу, не створюйте шуму та очікуйте подальших повідомлень.",
            "OK");
    }

    private void ActivateIncident(string caption, bool isDrill)
    {
        incidentActive = true;
        HeaderSubtitle.Text = caption;
        SystemTitle.Text = isDrill ? "Навчальний режим" : "Активна тривога";
        SystemSubtitle.Text = isDrill ? "Тренування без реальної загрози" : $"{GetScopeText()} - {GetVibrationText()}";
        LiveBadge.Text = isDrill ? "● DRILL" : "● ALERT";
        AlertTitle.Text = isDrill ? "Навчальна тривога" : "Тихий сигнал активний";
        AlertSubtitle.Text = isDrill ? "Перевірка маршруту оповіщення" : "Очікуйте підтвердження та не створюйте шуму";
        ModeBadge.Text = isDrill ? "DR" : "SOS";
        InstructionEmoji.Text = isDrill ? "🧪" : "🤫";
        PrimaryStatusLabel.Text = isDrill ? "Тренувальне оповіщення активне" : "Беззвучне сповіщення активне";
        SecondaryStatusLabel.Text = isDrill ? "Дані потрапляють у навчальний журнал" : "Вкажіть статус або очікуйте подальших повідомлень";
        AlarmButton.Text = isDrill ? "🧪\nНАВЧАЛЬНА\nТРИВОГА" : "🚨\nСИГНАЛ\nНАДІСЛАНО";
        AlarmButton.BackgroundColor = isDrill ? Color.FromArgb("#C99318") : Color.FromArgb("#B73535");
        HoldProgress.Opacity = 0;
        HoldProgress.Rotation = -90;
    }

    private void UpdateIncidentUi()
    {
        HeaderSubtitle.Text = "Коледж онлайн - тихий режим";
        SystemTitle.Text = "Система готова";
        SystemSubtitle.Text = "Остання перевірка: щойно";
        LiveBadge.Text = "● LIVE";
        AlertTitle.Text = "Тихе сповіщення";
        AlertSubtitle.Text = "Утримуйте кнопку, щоб передати сигнал черговому";
        ModeBadge.Text = "OK";
        InstructionEmoji.Text = "✅";
        PrimaryStatusLabel.Text = "Активних тривог немає";
        SecondaryStatusLabel.Text = "Система працює разом з офіційними процедурами безпеки";
        AlarmButton.Text = "🚨\nТРИВОГА\nутримуйте 3 с";
        AlarmButton.BackgroundColor = Color.FromArgb("#E93D3D");
        HoldProgress.Opacity = 0;
        HoldProgress.Rotation = -90;
    }

    private void UpdateRoleUi()
    {
        StudentPanel.IsVisible = !adminMode;
        AdminPanel.IsVisible = adminMode;

        StudentRoleButton.BackgroundColor = adminMode ? Colors.Transparent : Color.FromArgb("#2467DE");
        StudentRoleButton.TextColor = adminMode ? Color.FromArgb("#7A8AA5") : Colors.White;
        AdminRoleButton.BackgroundColor = adminMode ? Color.FromArgb("#2467DE") : Colors.Transparent;
        AdminRoleButton.TextColor = adminMode ? Colors.White : Color.FromArgb("#7A8AA5");

        if (!incidentActive)
        {
            AlertSubtitle.Text = adminMode
                ? "Оберіть аудиторію, режим сповіщення та підтвердіть інцидент"
                : "Утримуйте кнопку, щоб передати сигнал черговому";
        }
    }

    private void UpdateCounters()
    {
        ReceivedCountLabel.Text = receivedCount.ToString();
        HelpCountLabel.Text = helpCount.ToString();
        CoverageCountLabel.Text = $"{coverage}%";
    }

    private void UpdateScopeUi()
    {
        SetSegment(ScopeBuildingButton, alertScope == AlertScope.Building);
        SetSegment(ScopeCampusButton, alertScope == AlertScope.Campus);
        SetSegment(ScopeAllButton, alertScope == AlertScope.College);
    }

    private void UpdateVibrationUi()
    {
        SetSegment(VibeSilentButton, vibrationMode == AlertVibration.Silent);
        SetSegment(VibeModerateButton, vibrationMode == AlertVibration.Moderate);
        SetSegment(VibeLoudButton, vibrationMode == AlertVibration.Loud);
        VibrationChip.Text = vibrationMode switch
        {
            AlertVibration.Silent => "🔕 Тихо",
            AlertVibration.Loud => "🔊 Гучно",
            _ => "📳 Помірна"
        };
    }

    private static void SetSegment(Button button, bool selected)
    {
        button.BackgroundColor = selected ? Color.FromArgb("#2467DE") : Colors.Transparent;
        button.TextColor = selected ? Colors.White : Color.FromArgb("#7A8AA5");
        button.BorderColor = selected ? Color.FromArgb("#2467DE") : Color.FromArgb("#AFC4E6");
        button.BorderWidth = selected ? 0 : 1;
    }

    private async Task ResetHoldVisuals()
    {
        await Task.WhenAll(
            AlarmButton.ScaleToAsync(1, 120, Easing.CubicOut),
            HoldProgress.FadeToAsync(0, 120, Easing.CubicOut));
        HoldProgress.Rotation = -90;
    }

    private void AddEvent(string text)
    {
        EventLogLabel.Text = $"{DateTime.Now:HH:mm}  {text}\n{EventLogLabel.Text}";
    }

    private TimeSpan GetVibrationDuration() => vibrationMode switch
    {
        AlertVibration.Silent => TimeSpan.Zero,
        AlertVibration.Loud => TimeSpan.FromMilliseconds(1000),
        _ => TimeSpan.FromMilliseconds(450)
    };

    private string GetScopeText() => alertScope switch
    {
        AlertScope.Campus => "Територія коледжу",
        AlertScope.College => "Весь коледж",
        _ => "Корпус Б"
    };

    private string GetVibrationText() => vibrationMode switch
    {
        AlertVibration.Silent => "повністю беззвучно",
        AlertVibration.Loud => "гучний режим",
        _ => "помірна вібрація"
    };

    private static void Pulse(TimeSpan duration)
    {
        if (duration > TimeSpan.Zero && Vibration.Default.IsSupported)
        {
            Vibration.Default.Vibrate(duration);
        }
    }

    private enum AlertScope
    {
        Building,
        Campus,
        College
    }

    private enum AlertVibration
    {
        Silent,
        Moderate,
        Loud
    }
}

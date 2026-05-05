using System.Threading;
using System.Threading.Tasks;
using CollegeAlert.Services;
using Plugin.LocalNotification;
using Plugin.LocalNotification.Core.Models;

namespace CollegeAlert;

public partial class MainPage : ContentPage
{
    private CancellationTokenSource? holdTokenSource;
    private bool isHolding;
    private AlertService alertService;
    
    private string selectedBuilding = "Верхній корпус";
    private string selectedFloor = "1 поверх";

    public MainPage()
    {
        InitializeComponent();
        Routing.RegisterRoute("ActiveAlertPage", typeof(ActiveAlertPage));

        alertService = new AlertService();
        _ = InitializeAsync();

        alertService.AlertReceived += async (title, building, floor) =>
        {
            if (Preferences.Get("Role", "Student") == "Admin") return;

            await MainThread.InvokeOnMainThreadAsync(async () => {
                await Shell.Current.GoToAsync($"ActiveAlertPage?Building={Uri.EscapeDataString(building)}&Floor={Uri.EscapeDataString(floor)}");
            });
        };

        alertService.IncidentConfirmed += (building, floor) =>
        {
            if (Preferences.Get("Role", "Student") == "Admin") return;

            // Відправляємо локальне сповіщення (банер + звук)
            var request = new NotificationRequest
            {
                NotificationId = 1000,
                Title = "УВАГА: ТРИВОГА!",
                Description = $"Підтверджено: {building}, {floor}. Терміново пройдіть в укриття!",
                Schedule = new NotificationRequestSchedule
                {
                    NotifyTime = DateTime.Now.AddSeconds(1) // Додаємо 1 секунду, щоб iOS встигла обробити до заморозки
                }
            };
            LocalNotificationCenter.Current.Show(request);

            MainThread.BeginInvokeOnMainThread(() => {
                // Пульсуюча вібрація на 15 секунд для підтримки iOS
                Task.Run(async () => {
                    for (int i = 0; i < 15; i++) {
                        if (Vibration.Default.IsSupported)
                            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(500));
                        await Task.Delay(1000);
                    }
                });
            });
        };

        var pointerGesture = new PointerGestureRecognizer();
        pointerGesture.PointerPressed += OnPointerPressed;
        pointerGesture.PointerReleased += OnPointerReleased;
        pointerGesture.PointerExited += OnPointerReleased;
        AlarmButton.GestureRecognizers.Add(pointerGesture);
    }

    private async Task InitializeAsync()
    {
        try { 
            if (await LocalNotificationCenter.Current.AreNotificationsEnabled() == false)
            {
                await LocalNotificationCenter.Current.RequestNotificationPermission();
            }
            await alertService.Initialize("Student"); 
        } catch { }
    }

    private void OnBuildingSelected(object sender, EventArgs e)
    {
        var btn = (Button)sender;
        selectedBuilding = btn.Text == "Верхній" ? "Верхній корпус" : "Нижній корпус";
        BuildingLabel.Text = selectedBuilding;
        
        UpperTab.BackgroundColor = btn == UpperTab ? Color.FromArgb("#EFF6FF") : Colors.Transparent;
        UpperTab.TextColor = btn == UpperTab ? Color.FromArgb("#2563EB") : Color.FromArgb("#64748B");
        
        LowerTab.BackgroundColor = btn == LowerTab ? Color.FromArgb("#EFF6FF") : Colors.Transparent;
        LowerTab.TextColor = btn == LowerTab ? Color.FromArgb("#2563EB") : Color.FromArgb("#64748B");
    }

    private void OnFloorSelected(object sender, EventArgs e)
    {
        var btn = (Button)sender;
        selectedFloor = $"{btn.Text} поверх";
        FloorLabel.Text = selectedFloor;

        F1.BackgroundColor = F2.BackgroundColor = F3.BackgroundColor = Colors.Transparent;
        F1.TextColor = F2.TextColor = F3.TextColor = Color.FromArgb("#64748B");

        btn.BackgroundColor = Color.FromArgb("#EFF6FF");
        btn.TextColor = Color.FromArgb("#2563EB");
    }

    private async void OnPointerPressed(object? sender, PointerEventArgs e)
    {
        if (isHolding) return;
        isHolding = true;
        holdTokenSource?.Cancel();
        holdTokenSource = new CancellationTokenSource();
        var token = holdTokenSource.Token;

        try {
            await AlarmButton.ScaleToAsync(0.95, 100, Easing.CubicOut);
            var animation = new Animation(v => ProgressRing.StrokeDashOffset = v, 675, 0);
            animation.Commit(this, "RingAnimation", length: 1200, easing: Easing.Linear);
            await Task.Delay(1200, token);
            
            if (!token.IsCancellationRequested)
            {
                await TriggerAlert();
            }
        } catch (TaskCanceledException) { 
        } catch { 
        } finally { 
            await ResetVisuals(); 
        }
    }

    private void OnPointerReleased(object? sender, PointerEventArgs e)
    {
        holdTokenSource?.Cancel();
        isHolding = false;
    }

    private async Task TriggerAlert()
    {
        if (Vibration.Default.IsSupported) Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(500));
        try {
            await alertService.SendSignal(selectedBuilding, selectedFloor);
            await DisplayAlert("Сигнал надіслано", $"Локація: {selectedBuilding}, {selectedFloor}", "OK");
        } catch {
            await DisplayAlert("Помилка", "Не вдалося відправити сигнал. Перевірте з'єднання.", "OK");
        }
    }

    private async Task ResetVisuals()
    {
        this.AbortAnimation("RingAnimation");
        ProgressRing.StrokeDashOffset = 675;
        await AlarmButton.ScaleToAsync(1.0, 100, Easing.CubicIn);
        isHolding = false;
    }
}

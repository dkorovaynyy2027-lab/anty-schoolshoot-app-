using CollegeAlert.Services;

namespace CollegeAlert;

[QueryProperty(nameof(Building), "Building")]
[QueryProperty(nameof(Floor), "Floor")]
public partial class ActiveAlertPage : ContentPage
{
    private AlertService alertService;

    public string Building { set => LocationLabel.Text = $"{Uri.UnescapeDataString(value)} - {FloorLabelText}"; }
    
    private string FloorLabelText = "";
    public string Floor { 
        set {
            FloorLabelText = Uri.UnescapeDataString(value);
            // Re-assign Building to update the full text if Floor is set after Building
            if (LocationLabel.Text != null && LocationLabel.Text.Contains(" - "))
            {
                LocationLabel.Text = LocationLabel.Text.Split(" - ")[0] + " - " + FloorLabelText;
            }
        } 
    }

    public ActiveAlertPage()
    {
        InitializeComponent();
        alertService = new AlertService();
        _ = alertService.Initialize("Student");

        alertService.AlertEnded += () =>
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("Відбій", "Тривогу успішно скасовано.", "OK");
                await Shell.Current.GoToAsync("//MainPage");
            });
        };
    }

    private async void OnSafeClicked(object sender, EventArgs e)
    {
        await alertService.UpdateStudentStatus("Safe");
        await DisplayAlert("Статус оновлено", "Ви відмічені як 'В безпеці'.", "OK");
    }

    private async void OnDangerClicked(object sender, EventArgs e)
    {
        await alertService.UpdateStudentStatus("Danger");
        await DisplayAlert("ДОПОМОГА", "Ваш запит надіслано адміну. Залишайтесь на місці!", "OK");
    }

    private async void OnAwayClicked(object sender, EventArgs e)
    {
        await alertService.UpdateStudentStatus("Away");
        await DisplayAlert("Статус оновлено", "Ви відмічені як такі, що не в корпусі.", "OK");
    }

    private async void OnBackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }
}

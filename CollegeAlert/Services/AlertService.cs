using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;

namespace CollegeAlert.Services;

public class SignalDto {
    public string Building { get; set; } = "";
    public string Floor { get; set; } = "";
}

public class StatsDto {
    public int SafeCount { get; set; }
    public int DangerCount { get; set; }
    public int AwayCount { get; set; }
}

public class AlertService
{
    private HubConnection? hubConnection;
    public event Action<string, string, string>? AlertReceived; // title, building, floor
    public event Action<string, string>? IncidentConfirmed;
    public event Action? AlertEnded;
    public event Action<SignalDto>? PendingSignalReceived;
    public event Action<StatsDto>? StatsUpdated;

    public string DeviceId { get; private set; }

    public AlertService()
    {
        DeviceId = Preferences.Get("DeviceId", "");
        if (string.IsNullOrEmpty(DeviceId))
        {
            DeviceId = Guid.NewGuid().ToString();
            Preferences.Set("DeviceId", DeviceId);
        }
    }

    public async Task Initialize(string role)
    {
        // Використовуємо локальну IP-адресу вашого Mac, щоб реальні пристрої могли підключитися через Wi-Fi
        string url = "http://192.168.0.102:5050/alerthub";

        hubConnection = new HubConnectionBuilder()
            .WithUrl(url)
            .WithAutomaticReconnect()
            .Build();

        hubConnection.On<string, string, string>("AlertConfirmed", (incidentId, building, floor) => {
            AlertReceived?.Invoke("АКТИВНА ТРИВОГА", building, floor);
            IncidentConfirmed?.Invoke(building, floor);
        });
        hubConnection.On("AlertEnded", () => AlertEnded?.Invoke());
        hubConnection.On<SignalDto>("ReceivedPendingSignal", (s) => PendingSignalReceived?.Invoke(s));
        hubConnection.On<StatsDto>("UpdateStats", (s) => StatsUpdated?.Invoke(s));

        try {
            await hubConnection.StartAsync();
            await hubConnection.InvokeAsync("JoinGroup", role);
        } catch { }
    }

    public async Task SendSignal(string building, string floor) => 
        await hubConnection?.InvokeAsync("SendAlertSignal", building, floor)!;

    public async Task UpdateStudentStatus(string status) => 
        await hubConnection?.InvokeAsync("UpdateStatus", DeviceId, status)!;

    public async Task ConfirmAlert(string id, string building, string floor) => 
        await hubConnection?.InvokeAsync("ConfirmAlert", id, building, floor)!;

    public async Task EndAlert() => 
        await hubConnection?.InvokeAsync("EndAlert")!;
}

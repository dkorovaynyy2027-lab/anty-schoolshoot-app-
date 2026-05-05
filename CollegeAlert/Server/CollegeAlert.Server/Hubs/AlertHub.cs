using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using System;

namespace CollegeAlert.Server.Hubs;

public class SignalDto {
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string Building { get; set; } = "";
    public string Floor { get; set; } = "";
}

public class StatsDto {
    public int SafeCount { get; set; }
    public int DangerCount { get; set; }
    public int AwayCount { get; set; }
}

public class AlertHub : Hub
{
    private const string AdminsGroup = "Admins";
    private readonly ILogger<AlertHub> logger;
    
    // Key: DeviceId, Value: Status (Safe, Danger, Away)
    private static ConcurrentDictionary<string, string> _studentStatuses = new();
    
    // Store pending signals until an admin confirms an incident
    private static List<SignalDto> _pendingSignals = new();

    public AlertHub(ILogger<AlertHub> logger)
    {
        this.logger = logger;
    }

    public override Task OnConnectedAsync()
    {
        logger.LogInformation(">>> Клієнт підключився: {ConnectionId}", Context.ConnectionId);
        return base.OnConnectedAsync();
    }

    public async Task SendAlertSignal(string building, string floor)
    {
        logger.LogInformation(">>> СИГНАЛ SOS: {Building}, {Floor} від {ConnectionId}", building, floor, Context.ConnectionId);
        var signal = new SignalDto { Building = building, Floor = floor };
        _pendingSignals.Add(signal);
        await Clients.Group(AdminsGroup).SendAsync("ReceivedPendingSignal", signal);
    }

    public async Task UpdateStatus(string deviceId, string status)
    {
        logger.LogInformation(">>> ОНОВЛЕННЯ СТАТУСУ: {Status} для пристрою {DeviceId} (ConnectionId: {ConnectionId})", status, deviceId, Context.ConnectionId);
        _studentStatuses[deviceId] = status;
        await BroadcastStats();
    }

    public async Task ConfirmAlert(string incidentId, string building, string floor)
    {
        logger.LogInformation(">>> ПІДТВЕРДЖЕННЯ ТРИВОГИ: {IncidentId} ({Building}, {Floor})", incidentId, building, floor);
        _pendingSignals.Clear();
        _studentStatuses.Clear();
        await Clients.All.SendAsync("AlertConfirmed", incidentId, building, floor);
        await BroadcastStats();
    }

    public async Task EndAlert()
    {
        logger.LogInformation(">>> ВІДБІЙ ТРИВОГИ");
        _pendingSignals.Clear();
        _studentStatuses.Clear();
        await Clients.All.SendAsync("AlertEnded");
        await BroadcastStats();
    }

    private async Task BroadcastStats()
    {
        var stats = new StatsDto {
            SafeCount = _studentStatuses.Values.Count(s => s == "Safe"),
            DangerCount = _studentStatuses.Values.Count(s => s == "Danger"),
            AwayCount = _studentStatuses.Values.Count(s => s == "Away")
        };
        logger.LogInformation(">>> СТАТИСТИКА: Safe={Safe}, Danger={Danger}, Away={Away}", stats.SafeCount, stats.DangerCount, stats.AwayCount);
        await Clients.Group(AdminsGroup).SendAsync("UpdateStats", stats);
    }

    public async Task JoinGroup(string role)
    {
        logger.LogInformation(">>> Роль призначена: {Role} для {ConnectionId}", role, Context.ConnectionId);
        if (role == "Admin")
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, AdminsGroup);
            
            // Send all unconfirmed pending signals to the newly connected admin
            foreach (var signal in _pendingSignals)
            {
                await Clients.Caller.SendAsync("ReceivedPendingSignal", signal);
            }
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        logger.LogInformation("<<< Клієнт відключився: {ConnectionId}", Context.ConnectionId);
        // Do NOT remove status from _studentStatuses here. This ensures stats persist across connection drops.
        await base.OnDisconnectedAsync(exception);
    }
}


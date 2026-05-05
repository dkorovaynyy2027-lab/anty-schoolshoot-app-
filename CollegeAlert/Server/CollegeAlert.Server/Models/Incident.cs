using System;
using System.ComponentModel.DataAnnotations;

namespace CollegeAlert.Server.Models;

public enum IncidentStatus
{
    Pending,    // Signal received, waiting for Admin approval
    Active,     // Confirmed by Admin, broadcasted to everyone
    Resolved,   // Danger cleared
    Cancelled   // False alarm
}

public enum IncidentType
{
    Emergency,
    Drill
}

public class Incident
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public IncidentType Type { get; set; } = IncidentType.Emergency;
    public IncidentStatus Status { get; set; } = IncidentStatus.Pending;
    public string Building { get; set; } = string.Empty;
    public string Floor { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ConfirmedAt { get; set; }
}

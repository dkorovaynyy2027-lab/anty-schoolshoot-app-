using System;
using System.ComponentModel.DataAnnotations;

namespace CollegeAlert.Server.Models;

public class StatusReport
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid IncidentId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty; // "Safe", "Help", "NotHere"
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}

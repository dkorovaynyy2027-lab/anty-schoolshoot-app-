namespace CollegeAlert.Services;

public static class AppLog
{
    public static void Info(string area, string message, params (string Key, object? Value)[] data)
    {
        Write("INFO", area, message, data);
    }

    public static void Warn(string area, string message, params (string Key, object? Value)[] data)
    {
        Write("WARN", area, message, data);
    }

    public static void Error(string area, string message, Exception exception, params (string Key, object? Value)[] data)
    {
        var extendedData = data
            .Concat(new[]
            {
                ("error", (object?)exception.Message),
                ("type", exception.GetType().Name),
                ("inner", exception.InnerException?.Message)
            })
            .ToArray();

        Write("ERROR", area, message, extendedData);
    }

    public static void Tap(string page, string control, params (string Key, object? Value)[] data)
    {
        Write("TAP", page, control, data);
    }

    private static void Write(string level, string area, string message, params (string Key, object? Value)[] data)
    {
        var platform = DeviceInfo.Platform.ToString();
        var device = DeviceInfo.Name;
        var details = data.Length == 0
            ? string.Empty
            : " | " + string.Join(" | ", data.Select(item => $"{item.Key}={item.Value ?? "null"}"));

        Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] [{level}] [{platform}] [{device}] [{area}] {message}{details}");
    }
}

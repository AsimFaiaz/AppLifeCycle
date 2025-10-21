// AppLifecycleTracker.cs
// Tracks application start, exit, uptime, crashes, and restarts.
// Author: Asim Faiaz
// License: MIT

using System;
using System.IO;
using System.Text.Json;
using System.Threading; 
#nullable enable   //Remove with proper guard

public sealed class LifecycleRecord
{
    public DateTimeOffset LastStart { get; set; }
    public DateTimeOffset? LastExit { get; set; }
    public bool WasCleanExit { get; set; }
    public int RestartCount { get; set; }
    public string LastExitReason { get; set; } = "Unknown";
}

public sealed class AppLifecycleTracker : IDisposable
{
    private readonly string _filePath;
    private LifecycleRecord _record;
    private readonly DateTimeOffset _started = DateTimeOffset.UtcNow;
    private bool _disposed;

    public AppLifecycleTracker(string? filePath = null)
    {
        _filePath = filePath ?? Path.Combine(AppContext.BaseDirectory, "lifecycle.json");
        _record = LoadState();

        _record.RestartCount++;
        _record.LastStart = DateTimeOffset.UtcNow;
        _record.LastExit = null;
        _record.WasCleanExit = false;
        _record.LastExitReason = "Running";

        SaveState();
        AppDomain.CurrentDomain.ProcessExit += (_, _) => OnExit("CleanExit");
        AppDomain.CurrentDomain.UnhandledException += (_, e) => OnCrash(e.ExceptionObject?.ToString());
    }

    private void OnExit(string reason)
    {
        _record.LastExit = DateTimeOffset.UtcNow;
        _record.WasCleanExit = reason == "CleanExit";
        _record.LastExitReason = reason;
        SaveState();
    }

    private void OnCrash(string? exception)
    {
        _record.LastExit = DateTimeOffset.UtcNow;
        _record.WasCleanExit = false;
        _record.LastExitReason = $"Crash: {exception?.Split('\n')[0]}";
        SaveState();
    }

    private LifecycleRecord LoadState()
    {
        if (File.Exists(_filePath))
        {
            var json = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<LifecycleRecord>(json) ?? new LifecycleRecord();
        }
        return new LifecycleRecord();
    }

    private void SaveState()
    {
        var json = JsonSerializer.Serialize(_record, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(_filePath, json);
    }

    public TimeSpan GetUptime() => DateTimeOffset.UtcNow - _started;

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        OnExit("Dispose");
    }
}


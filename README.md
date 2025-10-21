<section id="applifecycle-overview">
  <h1>AppLifecycleTracker</h1>

  ![Status](https://img.shields.io/badge/status-stable-blue)
  ![Build](https://img.shields.io/badge/build-passing-brightgreen)
  ![License](https://img.shields.io/badge/license-MIT-lightgrey)
  
  <h2>Overview</h2>
  <p>
    <strong>AppLifecycleTracker</strong> is a lightweight, dependency-free C# utility that automatically 
    tracks your application's <strong>startups</strong>, <strong>exits</strong>, <strong>crashes</strong>, and 
    <strong>uptime</strong> -- storing all lifecycle data in a simple JSON file.
  </p>

  <p>
    It’s perfect for small tools, daemons, background services, or game engines 
    that need to know how long they’ve been running, how many times they’ve restarted,
    or whether the last exit was clean or due to a crash.
  </p>

  <h2>Developer Note</h2>
  <p>
    This project is part of my series of standalone micro-libraries -- clean, 
    focused, single-file tools that demonstrate elegant problem-solving and 
    practical software design. Each project is 100% self-contained and open source.
  </p>

  <h2>Key Features</h2>
  <ul>
    <li><strong>Automatic tracking:</strong> Detects app start, exit, and crash automatically</li>
    <li><strong>Uptime logging:</strong> Records how long the app ran before shutting down</li>
    <li><strong>Crash detection:</strong> Catches unhandled exceptions and marks them in logs</li>
    <li><strong>Restart counter:</strong> Tracks how many times the app has been started</li>
    <li><strong>Persistent store:</strong> Saves lifecycle data to JSON for future runs</li>
    <li><strong>Single-file simplicity:</strong> No dependencies, just drop it into your project</li>
  </ul>

  <h2>How It Works</h2>
  <p>
    When your application starts, the tracker loads a local JSON state file (by default, <code>lifecycle.json</code>).
    It records a new startup timestamp and increments the restart counter.
  </p>

  <p>
    When the app exits normally, the tracker logs a clean shutdown.
    If an <strong>unhandled exception</strong> occurs, it logs a crash entry with the exception summary.
  </p>

  <h2>Example Usage</h2>
  <pre>
using System;

class Program
{
    static void Main()
    {
        using var tracker = new AppLifecycleTracker();

        Console.WriteLine("App started...");
        Console.WriteLine($"Uptime file: {Path.Combine(AppContext.BaseDirectory, "lifecycle.json")}");
        Console.WriteLine("Press Ctrl+C to exit or crash...");

        Thread.Sleep(5000);

        Console.WriteLine($"Uptime: {tracker.GetUptime():g}");
    }
}
  </pre>

  <h2>Sample JSON Output</h2>
  <pre>
  {
    "LastStart": "2025-10-20T11:00:00Z",
    "LastExit": "2025-10-20T11:05:03Z",
    "WasCleanExit": true,
    "RestartCount": 3,
    "LastExitReason": "CleanExit"
  }
  </pre>

  <h2>How It Helps</h2>
  <ul>
    <li><strong>Detect crashes:</strong> Identify whether the last shutdown was normal or due to an exception</li>
    <li><strong>Monitor uptime:</strong> Keep track of how long your app stays alive between restarts</li>
    <li><strong>Restart metrics:</strong> Log how many times your app has been restarted</li>
    <li><strong>Simple debugging:</strong> Quickly check crash reason or uptime via a single JSON file</li>
  </ul>

  <h2>Interface Summary</h2>
  <pre>
  public sealed class LifecycleRecord
  {
      public DateTimeOffset LastStart { get; set; }
      public DateTimeOffset? LastExit { get; set; }
      public bool WasCleanExit { get; set; }
      public int RestartCount { get; set; }
      public string LastExitReason { get; set; }
  }

  public sealed class AppLifecycleTracker : IDisposable
  {
      public AppLifecycleTracker(string? filePath = null);
      public TimeSpan GetUptime();
      public void Dispose(); // marks clean exit
  }
  </pre>

  <h2>Manual Testing - .NET Fiddle</h2>
  <pre>
// To test in .NET Fiddle, paste this code under your main file

public static class Program
{
    public static void Main()
    {
        using var tracker = new AppLifecycleTracker();

        Console.WriteLine("App started...");
        Console.WriteLine("Simulating runtime...");

        Thread.Sleep(3000);
        Console.WriteLine($"Uptime: {tracker.GetUptime():g}");

        // Uncomment to simulate crash:
        // throw new Exception("Simulated crash");
    }
}
  </pre>

  <h2>Tech Stack</h2>
  <pre>☑ C# (.NET 8 or newer)</pre>
  <pre>☑ System.Text.Json for persistence</pre>
  <pre>☑ No external dependencies</pre>

  <h2>Build Status</h2>
  <p>
    This project is a single-file demo utility and does not require a build system.
    Future updates may include GitHub Actions CI and test coverage reports.
  </p>

  <h2>License</h2>
  <p>
    Licensed under the <a href="LICENSE">MIT License</a>.
  </p>
</section>

using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace aiKart.Utils;
public class PerformanceMonitorAttribute : IActionFilter
{
  private Stopwatch stopwatch;
  private readonly ILogger<PerformanceMonitorAttribute> _logger;
  public PerformanceMonitorAttribute(ILogger<PerformanceMonitorAttribute> logger)
  {
    _logger = logger;
  }
  public void OnActionExecuting(ActionExecutingContext context)
  {
    // Set the current controller instance for the interceptor
    stopwatch = Stopwatch.StartNew();

  }

  public void OnActionExecuted(ActionExecutedContext context)
  {
    stopwatch.Stop();
    var classType = context.Controller.GetType();
    var methodName = GetMethodName(context.ActionDescriptor.DisplayName);
    var statusCode = context.Result;

    var elapsedTime = stopwatch.ElapsedMilliseconds;

    var logMessage = $"[{DateTime.Now}] class:{classType} method:{methodName} took {elapsedTime} ms, response code:{statusCode}";

    LogToFile(logMessage);

  }

  private string GetMethodName(string fullMethodName)
  {
    // Assuming the format is "Namespace.Class.Method"
    // Split by '.' and get the last part
    var parts = fullMethodName.Split('.');
    return parts.Length > 0 ? parts[parts.Length - 1] : fullMethodName;
  }

  private void LogToFile(string logMessage)
  {
    var filePath = "performance.log";

    if (File.Exists(filePath))
    {
      // Append log message to the file
      File.AppendAllText(filePath, logMessage + Environment.NewLine);
    }
    else
    {
      // Log a warning if the file doesn't exist
      _logger.LogWarning($"Log file not found at: {filePath}. Unable to log to file.");
    }
  }
}

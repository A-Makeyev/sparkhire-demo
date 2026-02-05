using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;

namespace Reports {
    public class ExtentReporting {
        private static ExtentReports? _extentReports;
        private static string? _currentReportPath;

        public static ExtentReports StartReporting() {
            if (_extentReports == null) {
                // Get GitHub Run ID or generate a random Local ID
                string? runId = Environment.GetEnvironmentVariable("GITHUB_RUN_NUMBER");
                if (string.IsNullOrEmpty(runId)) {
                    runId = "Local-" + new Random().Next(1000, 9999);
                }

                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                string folderName = $"Run-{runId}_{timestamp}";
                
                // Construct path to Results folder in project root
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Results", folderName);
                
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                _currentReportPath = Path.Combine(path, "index.html");
                var htmlReporter = new ExtentSparkReporter(_currentReportPath);
                
                htmlReporter.Config.DocumentTitle = "Automation Status Report";
                htmlReporter.Config.ReportName = "Regression Testing";
                htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Config.Theme.Standard;

                _extentReports = new ExtentReports();
                _extentReports.AttachReporter(htmlReporter);
                
                _extentReports.AddSystemInfo("Execution ID", runId);
                _extentReports.AddSystemInfo("Machine", Environment.MachineName);
            }
            return _extentReports;
        }

        public static void EndReporting() => _extentReports?.Flush();

        public static void OpenReport() {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
                if (_currentReportPath != null && File.Exists(_currentReportPath)) {
                    Process.Start(new ProcessStartInfo(_currentReportPath) { UseShellExecute = true });
                }
            } else {
                Console.WriteLine($"[INFO] Report generated at: {_currentReportPath}");
            }
        }

        public static string CaptureScreenshot(IWebDriver driver) {
            return ((ITakesScreenshot)driver).GetScreenshot().AsBase64EncodedString;
        }
    }
}

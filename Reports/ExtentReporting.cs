using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices; // Added for OS check
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;

namespace Reports {
    public class ExtentReporting {
        private static ExtentReports? _extentReports;
        private static string? _currentReportPath;

        public static ExtentReports StartReporting() {
            if (_extentReports == null) {
                string timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Results", timestamp);
                
                if (!Directory.Exists(path)) Directory.CreateDirectory(path);

                _currentReportPath = Path.Combine(path, "index.html");
                var htmlReporter = new ExtentSparkReporter(_currentReportPath);
                
                htmlReporter.Config.DocumentTitle = "Automation Status Report";
                htmlReporter.Config.ReportName = "Regression Testing";
                htmlReporter.Config.Theme = AventStack.ExtentReports.Reporter.Config.Theme.Standard;

                _extentReports = new ExtentReports();
                _extentReports.AttachReporter(htmlReporter);
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
                Console.WriteLine("[INFO] Skipping auto open report on Linux");
            }
        }

        public static string CaptureScreenshot(IWebDriver driver) {
            return ((ITakesScreenshot)driver).GetScreenshot().AsBase64EncodedString;
        }
    }
}

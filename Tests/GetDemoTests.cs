using Reports;
using AventStack.ExtentReports;
using ComeetDemoTests.Config;
using ComeetDemoTests.Utils;
using OpenQA.Selenium;
using ComeetDemoTests.Pages;
using NUnit.Framework.Interfaces;


namespace ComeetDemoTests.Tests {
    [TestFixture]
    public class GetDemoTests {
        private IWebDriver? _driver;
        private ExtentTest? _test;
        private static ExtentReports? _extent;

        [OneTimeSetUp]
        public void GlobalSetup() {
            _extent = ExtentReporting.StartReporting();
        }

        [SetUp]
        public void Setup() {
            _test = _extent?.CreateTest(TestContext.CurrentContext.Test.Name);
            _driver = WebDriverFactory.Create();
            
            string browserName = "Unknown";
            if (_driver is IHasCapabilities caps && caps.Capabilities != null) {
                browserName = caps.Capabilities.GetCapability("browserName")?.ToString() ?? "Unknown";
            }
            
            _test?.Log(Status.Info, $"[INFO] WebDriver launched: {browserName}");
            _driver.Navigate().GoToUrl(EnvConfig.BaseUrl);
            _test?.Log(Status.Info, $"BaseUrl: {EnvConfig.BaseUrl}");
        }

        [Test]
        public void FillGetDemoForm() {
            var homePage = new HomePage(_driver!);
            homePage.ClickGetDemo();
            SwitchToNewTab(_driver!);

            var getDemoPage = new GetDemoPage(_driver!);
            
            DateTime start = DateTime.Now;
            getDemoPage.WaitForPage();
            double duration = (DateTime.Now - start).TotalSeconds;

            string currentUrl = _driver!.Url;
            _test?.Log(Status.Info, $"Navigated to: {currentUrl}");
            
            if (duration > 3.0) {
                _test?.Log(Status.Warning, $"Get Demo page took {duration:F2}s to load!");
            } else {
                _test?.Log(Status.Pass, $"Get Demo page loaded in {duration:F2}s.");
            }

            getDemoPage.SelectCompanyRole();
            getDemoPage.FillContactDetails();
            getDemoPage.FillRandomizedBusinessDetails();
            getDemoPage.SubmitForm();
            
            string successScreen = ExtentReporting.CaptureScreenshot(_driver!);
            _test?.Pass("Form submitted successfully.", 
                MediaEntityBuilder.CreateScreenCaptureFromBase64String(successScreen).Build());
        }

        private void SwitchToNewTab(IWebDriver driver) {
            var originalWindow = driver.CurrentWindowHandle;
            var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(driver, TimeSpan.FromSeconds(10));
            wait.Until(d => d.WindowHandles.Count > 1);

            foreach (var handle in driver.WindowHandles) {
                if (handle != originalWindow) {
                    driver.SwitchTo().Window(handle);
                    break;
                }
            }
        }

        [TearDown]
        public void TearDown() {
            if (_driver != null) {
                var status = TestContext.CurrentContext.Result.Outcome.Status;

                if (status == TestStatus.Failed) {
                    string screenshot = ExtentReporting.CaptureScreenshot(_driver);
                    _test?.Fail($"Test Failed: {TestContext.CurrentContext.Result.Message}", 
                        MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshot).Build());
                }

                _driver.Quit();
                _driver.Dispose();
                _driver = null;
            }
        }

        [OneTimeTearDown]
        public void GlobalTeardown() {
            ExtentReporting.EndReporting();
            ExtentReporting.OpenReport();
        }
    }
}

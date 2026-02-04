using ComeetDemoTests.Config;
using ComeetDemoTests.Utils;
using OpenQA.Selenium;
using ComeetDemoTests.Pages;

namespace ComeetDemoTests.Tests
{
    public class GetDemoTests
    {
        private IWebDriver? _driver;

        [SetUp]
        public void Setup()
        {
            _driver = WebDriverFactory.Create();
            _driver.Navigate().GoToUrl(EnvConfig.BaseUrl);
        }

        [Test]
        public void GetDemo_FillForm_WithoutSubmission()
        {
            var homePage = new HomePage(_driver!);
            homePage.ClickGetDemo();

            SwitchToNewTab(_driver!);

            var getDemoPage = new GetDemoPage(_driver!);
            getDemoPage.WaitForPage();

            getDemoPage.SelectCompanyRole();
            getDemoPage.FillContactDetails();
            getDemoPage.FillRandomizedBusinessDetails();
            getDemoPage.SubmitForm();
        }

        private void SwitchToNewTab(IWebDriver driver)
        {
            var originalWindow = driver.CurrentWindowHandle;

            var wait = new OpenQA.Selenium.Support.UI.WebDriverWait(
                driver, TimeSpan.FromSeconds(10));

            wait.Until(d => d.WindowHandles.Count > 1);

            foreach (var handle in driver.WindowHandles)
            {
                if (handle != originalWindow)
                {
                    driver.SwitchTo().Window(handle);
                    break;
                }
            }
        }

        [TearDown]
        public void TearDown()
        {
            if (_driver != null)
            {
                try
                {
                    _driver.Quit();
                }
                finally
                {
                    _driver.Dispose();
                }
            }
        }
    }
}

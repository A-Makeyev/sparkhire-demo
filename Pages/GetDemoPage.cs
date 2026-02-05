using OpenQA.Selenium;
using System.Diagnostics;
using OpenQA.Selenium.Support.UI;

namespace ComeetDemoTests.Pages {
    public class GetDemoPage : BasePage {
        private readonly By RoleSelect = By.Id("role");
        private readonly By FirstNameInput = By.Name("firstname");
        private readonly By LastNameInput = By.Name("lastname");
        private readonly By CompanyInput = By.Name("company");
        private readonly By EmailInput = By.Name("email");
        private readonly By CompanySizeSelect = By.Name("company_size__c");
        private readonly By CountrySelect = By.Name("country");
        private readonly By ProductInterestSelect = By.Name("product_demo_interest");
        private readonly By SubmitButton = By.CssSelector("input[type='submit']");
        public GetDemoPage(IWebDriver driver) : base(driver) { }

        public void WaitForPage() {
            var stopwatch = Stopwatch.StartNew();

            WaitForVisible(RoleSelect);

            stopwatch.Stop();
            if (stopwatch.Elapsed.TotalSeconds > 3) {
                Console.WriteLine($"WARNING: Get Demo page took {stopwatch.Elapsed.TotalSeconds:F2}s to load.");
            }
        }

        public void SelectCompanyRole() {
            var select = new SelectElement(WaitForVisible(RoleSelect));
            select.SelectByValue("company");
        }

        public void FillContactDetails() {
            Driver.FindElement(FirstNameInput).SendKeys("Anatoly");
            Driver.FindElement(LastNameInput).SendKeys("Makeyev");
            Driver.FindElement(CompanyInput).SendKeys("Cloudbeat");
            Driver.FindElement(EmailInput).SendKeys("anatoly.makeyev@cloudbeat.io");
        }

        public void FillRandomizedBusinessDetails() {
            new SelectElement(WaitForVisible(CompanySizeSelect))
                .SelectByIndex(RandomIndex(1, 7));

            new SelectElement(WaitForVisible(CountrySelect))
                .SelectByText("Israel");

            new SelectElement(WaitForVisible(ProductInterestSelect))
                .SelectByValue("Video Interviews");
        }

        public void SubmitForm() {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(SubmitButton));
            // WaitForVisible(SubmitButton).Click();
            Thread.Sleep(2500);
        }

        private int RandomIndex(int min, int max) {
            return new Random().Next(min, max + 1);
        }
    }
}

using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;


namespace ComeetDemoTests.Pages {
    public abstract class BasePage {
        protected readonly IWebDriver Driver;
        protected readonly WebDriverWait Wait;

        protected BasePage(IWebDriver driver) {
            Driver = driver;
            Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }

        protected IWebElement WaitForVisible(By locator) {
            return Wait.Until(ExpectedConditions.ElementIsVisible(locator));
        }
    }
}

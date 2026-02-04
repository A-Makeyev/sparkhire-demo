using OpenQA.Selenium;

namespace ComeetDemoTests.Pages
{
    public class HomePage : BasePage
    {
        private readonly By GetDemoLink = By.XPath("//a[@title='Get Demo']");

        public HomePage(IWebDriver driver) : base(driver) { }

        public void ClickGetDemo()
        {
            var link = WaitForVisible(GetDemoLink);
            link.Click();
        }
    }
}

using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;
using dotenv.net;

namespace ComeetDemoTests.Utils
{
    public static class WebDriverFactory
    {
        static WebDriverFactory()
        {
            // Looks up folder tree until it finds .env
            DotEnv.Load(options: new DotEnvOptions(probeForEnv: true));
        }

        public static IWebDriver Create(string browser = "chrome")
        {
            var ciEnv = Environment.GetEnvironmentVariable("CI");
            bool isCi = string.Equals(ciEnv, "true", StringComparison.OrdinalIgnoreCase);

            IWebDriver driver;
            string browserInfo = "";

            switch (browser.ToLower())
            {
                case "chrome":
                    new DriverManager().SetUpDriver(new ChromeConfig());
                    var chromeOptions = new ChromeOptions();
                    chromeOptions.AddArgument("--no-sandbox");
                    chromeOptions.AddArgument("--disable-dev-shm-usage");
                    chromeOptions.AddArgument("--disable-gpu");

                    if (isCi)
                    {
                        chromeOptions.AddArgument("--headless=new");
                        chromeOptions.AddArgument("--window-size=1920,1080");
                        browserInfo = "Chrome (headless)";
                    }
                    else
                    {
                        browserInfo = "Chrome (UI)";
                    }

                    driver = new ChromeDriver(chromeOptions);
                    break;

                case "firefox":
                    new DriverManager().SetUpDriver(new FirefoxConfig());
                    var firefoxOptions = new FirefoxOptions();

                    if (isCi)
                    {
                        firefoxOptions.AddArgument("-headless");
                        browserInfo = "Firefox (headless)";
                    }
                    else
                    {
                        browserInfo = "Firefox (UI)";
                    }

                    driver = new FirefoxDriver(firefoxOptions);

                    // Set window size after creating Firefox driver in headless mode
                    if (isCi)
                        driver.Manage().Window.Size = new System.Drawing.Size(1920, 1080);
                    break;

                default:
                    throw new ArgumentException($"Unsupported browser: {browser}");
            }

            // Global implicit wait
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            // Maximize window if not CI
            if (!isCi)
            {
                driver.Manage().Window.Maximize();
            }

            // Print info about which browser was launched
            Console.WriteLine($"[INFO] WebDriver launched: {browserInfo}");

            return driver;
        }

        public static string GetBaseUrl() => GetEnv("BASE_URL", "https://www.comeet.com");

        public static string GetEnv(string key, string fallback = "")
        {
            return Environment.GetEnvironmentVariable(key) ?? fallback;
        }
    }
}

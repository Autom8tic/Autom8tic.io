using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoSolveClient;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
namespace Dashboard1.Bots
{
    class UniversalBot
    {
        IWebDriver driver;

        public void mainCheck()
        {
            var taskId = "";

        }

        public bool checkProductAvailability(string Name)
        {
            Proxy proxy = new Proxy();
            proxy.Kind = ProxyKind.Manual;
            proxy.IsAutoDetect = false;
            proxy.SslProxy = "216.229.60.65:8080";

            // Add a ChromeDriver-specific capability.

            var chromeOptions = new ChromeOptions();
            //chromeOptions.Proxy = proxy;
            chromeOptions.AddArgument("--ignore-certificate-errors");
            chromeOptions.AddArguments("--window-size=1920,1080");
            //chromeOptions.AddArguments("--headless");
            chromeOptions.PageLoadStrategy = PageLoadStrategy.Eager;
            //chromeOptions.AddArguments("--proxy-server='direct://'");
            //chromeOptions.AddArguments("--proxy-bypass-list=*");
            driver = new ChromeDriver(chromeOptions);
            //Navigation to a url and a look at the traffic logged in fiddler
            //driver.Navigate().GoToUrl("https://www.adidas.co.uk/men/");
            System.Threading.Thread.Sleep(5000);
            //driver.Url = "https://www.truescoopnews.com/";
            try
            {
                var ss = driver.PageSource;
                System.Threading.Thread.Sleep(5000);
               var newurl = driver.FindElement(By.XPath("//*[@id='top-part']/div/div/div/div[2]/div/div[1]/section[1]/div[2]/div[2]/h3/a")).GetAttribute("href");
                System.Threading.Thread.Sleep(5000);
                driver.Url = newurl;
                System.Threading.Thread.Sleep(10000);
                driver.SwitchTo().Frame(driver.FindElement(By.XPath("//*[@id='aswift_0']")));
                driver.FindElement(By.XPath("//*[@id='bannerB']/a")).Click();
                //var link = driver.FindElement(By.XPath("//img[contains(translate(@title, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz') , translate('" + Name + "', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'))]")).GetAttribute("href");
                
                driver.Quit();

                //driver.Url();
                return true;
            }
            catch
            {
                driver.Quit();
                return false;
            }
        }

    }
}

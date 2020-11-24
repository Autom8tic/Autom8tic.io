using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using static Dashboard1.ProfileSection;

namespace Dashboard1.Bots
{
    class YeezySupply
    {
        IWebDriver driver;
        public BitmapImage fetchImage(IWebDriver driver)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20000));
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("navigation-target-gallery")));
            var img = driver.FindElement(By.Id("navigation-target-gallery")).FindElement(By.TagName("img")).GetAttribute("src");
            return new BitmapImage(new Uri(img));

        }

        public string FindProduct(string sz, string pr, string url, string keyword)
        {
            var chromeOptions = new ChromeOptions();
            //chromeOptions.AddArguments("headless");
            // chromeOptions.AddArguments("--allow-insecure-localhost");
            driver = new ChromeDriver(chromeOptions);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2000));
            driver.Url = url;
            var sd = "";
            if (IsElementPresent(By.PartialLinkText(keyword)))
            {
               sd = driver.FindElement(By.PartialLinkText(keyword)).GetAttribute("href");
            }
            
            //Trefoil Essentials T-Shirt
           
            var name = char.ToUpper(keyword[0]) + keyword.ToLower().Substring(1);
            wait.Until(ExpectedConditions.ElementExists(By.XPath("//img[contains(@alt, '" + name + "')]")));
            var image = driver.FindElement(By.XPath("//img[contains(@alt, '" + name + "')]")).GetAttribute("src");
            driver.Close();
            driver.Dispose();
            driver.Quit();
            return image;


        }
        public string FindProduct1(string sz, string pr, string url, string keyword, string proxy = "")
        {
            var po = proxy.Split('|')[0];
            // Create proxy class object

            // Create proxy class object
            Proxy p = new Proxy();


            // Set HTTP Port to 7777

            // Create desired Capability object
            DesiredCapabilities cap = new DesiredCapabilities();


            // Pass proxy object p
            cap.SetCapability(CapabilityType.Proxy, p);
            var service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            
            ChromeOptions option = new ChromeOptions();
            p.Kind = ProxyKind.Manual;
            p.IsAutoDetect = false;
            //p.HttpProxy =
            //p.HttpProxy = po;
            p.SslProxy = po;
            option.AddArguments("--window-size=1920,1080");
            option.AddArguments("--disable-gpu");
            option.AddArguments("--disable-extensions");
            //option.Extensions("useAutomationExtension", false);
            //option.AddArguments("--proxy-server='direct://'");
            //option.AddArguments("--proxy-bypass-list=*");
            option.AddArguments("--start-maximized");
            //option.AddArguments("--headless");
            if (po == "NA" || po == "") { 
            
            } else { 
            option.Proxy = p;
            option.AddArgument("ignore-certificate-errors");

            }
            //option.AddArgument("--proxy-server=%s");
            option.AddArgument("--headless");

            driver = new ChromeDriver(service, option);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2000));
            driver.Navigate().GoToUrl(url);
            //wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='modal-root']/div/div/div/div[2]/div[1]/div[1]/div/label[2]")));

            //if (IsElementPresent(By.XPath("//*[@id='modal-root']/div/div/div/div[2]/div[1]/div[1]/div/label[2]")))
            //{
            //    driver.FindElement(By.XPath("//*[@id='modal-root']/div/div/div/div[2]/div[1]/div[1]/div/label[2]")).Click();
            //}
            //wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='modal-root']/div/div/div/div[2]/div[2]/button")));
            //driver.FindElement(By.XPath("//*[@id='modal-root']/div/div/div/div[2]/div[2]/button")).Click();
            keyword = keyword.ToUpper();
            //wait.Until(ExpectedConditions.ElementExists(By.PartialLinkText(keyword)));
            if (IsElementPresent(By.PartialLinkText(keyword)))
            {
                try
                {
                    //Trefoil Essentials T-Shirt
                    var sd = driver.FindElement(By.PartialLinkText(keyword)).GetAttribute("href");
                    var name = keyword.ToLower();// char.ToUpper(keyword[0]) + keyword.ToLower().Substring(1);
                                                 // wait.Until(ExpectedConditions.ElementExists(By.XPath("//img[contains(lower-case(@alt), '" + name + "')]")));
                                                 //wait.Until(ExpectedConditions.ElementExists(By.XPath("//a[contains(@href, '" + sd + "')]/following-sibling::img[1]")));

                    var image = driver.FindElement(By.XPath("//img[contains(translate(@alt, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz') , translate('" + name + "', 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'))]")).GetAttribute("src");
                    driver.Close();
                    driver.Dispose();
                    driver.Quit();
                    return image + "|" + sd;
                }
                catch (Exception ex)
                {
                    return "";
                }
            }
            else
            {
               driver.Close();
              driver.Dispose();
               driver.Quit();
                return "";
            }

            //if (sd != "")
            //{
            //    StartCheckout(sz, pr, sd);
            //}

        }


        public string StartCheckout(string sz, string pr, string url, string proxy = "")
        {
            var service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            ChromeOptions option = new ChromeOptions();
            //option.AddArgument("--headless");

            driver = new ChromeDriver(service, option);
            //driver = new ChromeDriver();
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2000));
            if (url.Contains("?"))
            {
                driver.Url = url;
            }
            else
            {
                driver.Url = url;

            }

            driver.FindElement(By.XPath("//*[@id='modal-root']/div/div/div/div[2]/div[1]/div[1]/div/label[2]")).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='modal-root']/div/div/div/div[2]/div[2]/button")));
            driver.FindElement(By.XPath("//*[@id='modal-root']/div/div/div/div[2]/div[2]/button")).Click();

            //fetchImage(driver);

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//button[@data-auto-id='add-to-bag']")));
            driver.FindElement(By.XPath("//button[@data-auto-id='add-to-bag']")).Click();

            wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='modal-root']/div/div/div/div[2]/div/section/div[3]/a[2]")));
            driver.FindElement(By.XPath("//*[@id='modal-root']/div/div/div/div[2]/div/section/div[3]/a[2]")).Click();

            //Add Delivery Info
            try
            {
                fillDelivery(driver, pr);
                driver.Quit();
                return "Success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        public void fillDelivery(IWebDriver driver, string pr)
        {
            //Add Delivery Info
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20000));

            //ProfileSection s = new ProfileSection();
            List<ProfileData> ss = getprofileList(pr);

            wait.Until(ExpectedConditions.ElementIsVisible(By.XPath("//*[@id='app']/div/div/div/div/div[2]/div/main/form/div/div[1]/div/div[1]/div/div/div[1]/input")));
            driver.FindElement(By.XPath("//*[@id='app']/div/div/div/div/div[2]/div/main/form/div/div[1]/div/div[1]/div/div/div[1]/input")).SendKeys(ss[0].firstname);

            //wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='modal-root']/div/div/div/div[2]/div/section/div[3]/a[2]"/)));
            driver.FindElement(By.XPath("//*[@id='app']/div/div/div/div/div[2]/div/main/form/div/div[1]/div/div[2]/div/div/div/input")).SendKeys(ss[0].lastname);

            //wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='modal-root']/div/div/div/div[2]/div/section/div[3]/a[2]")));
            driver.FindElement(By.XPath("//*[@id='app']/div/div/div/div/div[2]/div/main/form/div/div[1]/div/div[3]/div/div/div[1]/input")).SendKeys(ss[0].address1);

            // wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='modal-root']/div/div/div/div[2]/div/section/div[3]/a[2]")));
            driver.FindElement(By.XPath("//*[@id='app']/div/div/div/div/div[2]/div/main/form/div/div[1]/div/div[4]/div/div/div[1]/input")).SendKeys(ss[0].apt);

            //wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='modal-root']/div/div/div/div[2]/div/section/div[3]/a[2]")));
            driver.FindElement(By.XPath("//*[@id='app']/div/div/div/div/div[2]/div/main/form/div/div[1]/div/div[5]/div/div/div/input")).SendKeys(ss[0].city);

            //wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='modal-root']/div/div/div/div[2]/div/section/div[3]/a[2]")));
            driver.FindElement(By.XPath("//*[@id='app']/div/div/div/div/div[2]/div/main/form/div/div[1]/div/div[6]/div/div/div[1]/input")).SendKeys(ss[0].zipcode);

            //wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='modal-root']/div/div/div/div[2]/div/section/div[3]/a[2]")));
            driver.FindElement(By.XPath("//*[@id='app']/div/div/div/div/div[2]/div/main/form/div/div[4]/div/div[1]/div/div/div[1]/input")).SendKeys(ss[0].phone);

            //wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='modal-root']/div/div/div/div[2]/div/section/div[3]/a[2]")));
            driver.FindElement(By.XPath("//*[@id='app']/div/div/div/div/div[2]/div/main/form/div/div[4]/div/div[2]/div/div/div/input")).SendKeys(ss[0].email);

            //wait.Until(ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id='app']/div/div/div/div/div[2]/div/main/div[9]/button")));
            driver.FindElement(By.XPath("//*[@id='app']/div/div/div/div/div[2]/div/main/div[9]/button")).Click();
            wait.Until(ExpectedConditions.ElementIsVisible(By.Name("card.number")));

            driver.FindElement(By.Name("card.number")).SendKeys(ss[0].cardnumber);
            driver.FindElement(By.Name("card.holder")).SendKeys(ss[0].cardholder);
            driver.FindElement(By.XPath("//input[@data-auto-id='expiry-date-field']")).SendKeys(ss[0].expiry);
            driver.FindElement(By.Name("card.cvv")).SendKeys(ss[0].cvv);
            driver.FindElement(By.XPath("//button[@data-auto-id='place-order-button']")).Click();
        }
        private bool IsElementPresent(By by)
        {
            try
            {
                System.Threading.Thread.Sleep(3000);
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2000));
                wait.Until(ExpectedConditions.ElementIsVisible(by));
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        public void StopProcess()
        {
            driver.Close();
            driver.Quit();
        }
    }
}

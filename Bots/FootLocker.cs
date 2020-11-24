using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dashboard1.ProfileSection;

namespace Automa8tic.Bots
{
    class FootLocker
    {
        public static string initiateDriver(string pr, string delay, string proxy = "", string keywords = "", string size = "", string color = "", BackgroundWorker bw = null, bool browser= false)
        {
            try
            {
                List<Cookie> lst = new List<Cookie>();
                ////foreach (var cook in cookies)
                ////{
                ////    Cookie ss = new Cookie(cook.Name, cook.Value, "www.supremenewyork.com", "/", DateTime.Now.AddDays(1));
                ////    lst.Add(ss);
                ////}

                IWebDriver driver;

                var service = ChromeDriverService.CreateDefaultService();
                service.HideCommandPromptWindow = true;
                var useragents = new List<string> { "Mozilla/5.0 (Windows NT 4.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/37.0.2049.0 Safari/537.36", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/33.0.1750.517 Safari/537.36", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_8_2) AppleWebKit/537.17 (KHTML, like Gecko) Chrome/24.0.1309.0 Safari/537.17" };
                int index = new Random().Next(useragents.Count);
                var name = useragents[index];
                useragents.RemoveAt(index);

                ChromeOptions option = new ChromeOptions();

                //option.AddArgument("--ignore-certificate-errors");
                option.AddArguments("--window-size=1529,800");
                //option.AddArguments("--user-data-dir=C:/Users/intel/AppData/Local/Google/Chrome/User Data");

                option.AddArguments("--user-agent='" + name + "'");
                if (!browser)
                {
                    option.AddArguments("--headless");
                }
                if (proxy != "" && proxy != "NA")
                {
                    var po = proxy.Split('|')[0];
                    Proxy p = new Proxy();

//                    option.AddArguments("--headless");
                    p.Kind = ProxyKind.Manual;
                    p.IsAutoDetect = false;
                    var tpr = proxy.Replace("|", "").Split(':');
                    if (tpr.Length > 2)
                    {
                        p.SslProxy = tpr[0] + ":" + tpr[1];
                        p.FtpProxy = tpr[0] + ":" + tpr[1];
                        p.SocksUserName = tpr[2];
                        p.SocksPassword = tpr[3];
                        //option.AddExtensions("Data/MultiPass-für-Authentifizierung-mit-HTTP-Basic_v0.9.1.crx");
                        //configureAuth(
                        //    tpr[0] + ":" + tpr[1],
                        //    "team",
                        //    "jeru");
                    }
                    else
                    {
                        p.SslProxy = proxy;
                    }
                    option.Proxy = p;
                }
                //option.PageLoadStrategy = PageLoadStrategy.Normal;

                driver = new ChromeDriver(service, option);
                var _cookies = driver.Manage().Cookies.AllCookies;
                driver.Manage().Cookies.DeleteAllCookies();

                //var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2000));

                var result = "";
                if (keywords.Contains("http"))
                {
                    driver.Navigate().GoToUrl(keywords);
                }
                else
                {
                    driver.Navigate().GoToUrl("https://www.footlocker.com/release-dates");
                }
                foreach (Cookie cookie in lst)
                {
                    driver.Manage().Cookies.AddCookie(cookie);
                }
                //option.AddArguments("--headless");
                //option.PageLoadStrategy = PageLoadStrategy.Normal;
                var IsProductFind = false;
                if (keywords.Contains("http")) {
                    IsProductFind = true;
                } else { 
                    IsProductFind = FindProduct(keywords, driver);
                }

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2000));
                if (IsProductFind)
                {
                    bw.ReportProgress(10);
                    try {
                        wait.Until(ExpectedConditions.ElementExists(By.ClassName("closeButtonWhite")));
                        driver.FindElement(By.ClassName("closeButtonWhite")).Click();
                    }
                    catch
                    {

                    }
                    try
                    {
                        //wait.Until(ExpectedConditions.ElementExists(By.Id("ccpaAgree")));
                        driver.FindElement(By.Id("ccpaAgree")).Click();
                    }
                    catch
                    {

                    }

                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    string title = (string)js.ExecuteScript("return document.getElementsByClassName('c-promotional-banner')[0].remove();");


                    wait.Until(ExpectedConditions.ElementExists(By.TagName("klarna-placement")));
                    string title1 = (string)js.ExecuteScript("return document.getElementsByTagName('klarna-placement')[0].remove();");


                    //driver.FindElement(By.Id("ProductDetails_radio_size_065")).Click();
                    driver.FindElement(By.XPath("//*[@id='ProductDetails']/div[5]/div[2]/fieldset/div/div[4]")).Click();
                    wait.Until(ExpectedConditions.ElementExists(By.ClassName("ProductDetails-form__action")));
                    driver.FindElement(By.ClassName("ProductDetails-form__action")).Click();
                    bw.ReportProgress(50);
                    System.Threading.Thread.Sleep(2000);
                    driver.Navigate().GoToUrl("https://www.footlocker.com/cart");
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("c-loading-curtain")));
                    wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='main']/div/div[3]/div/div/div/div[2]/div[2]/a")));
                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("c-loading-curtain")));
                    //wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("ReactModal_Overlay")));
                    driver.FindElement(By.XPath("//*[@id='main']/div/div[3]/div/div/div/div[2]/div[2]/a")).Click();
                    try
                    {
                        bw.ReportProgress(50);
                        result = fillDelivery(driver, pr, delay, bw);
                        driver.Navigate().GoToUrl("https://www.footlocker.com/checkout");
                      
                        //if (!result.Contains("success"))
                        //{
                        //    driver.Navigate().GoToUrl("https://www.footlocker.com/checkout");
                        //    bw.ReportProgress(50);
                        //    result = fillDelivery(driver, pr, delay, bw);
                        //}
                        bw.ReportProgress(90);
                    }
                    catch (Exception ex)
                    {
                        result = ex.Message;
                        driver.Quit();
                    }
                    //Guest Checkout
                    //closeButtonWhite 
                }
                else
                {
                    bw.ReportProgress(-2);
                    result = "Not found";
                     driver.Quit();
                    return result;
                }

               
                return result;
            }
            catch (Exception ex)
            {
                return "Failed!";
            }
        }


        public static bool FindProduct(string keywords, IWebDriver driver)
        {
            try {
                driver.FindElement(By.Id("releaseCalendarTabs-launched-tab")).Click();
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2000));
                driver.FindElement(By.PartialLinkText(keywords)).Click();
                return true;
            }
            catch
            {
                return false;
            }
        }


        public static string fillDelivery(IWebDriver driver, string pr, string delay, BackgroundWorker bw)
        {

            var sitekey = "6LeWwRkUAAAAAOBsau7KpuC9AV-6J8mhw4AjC3Xz";
            var apiKey = "0204a048b7b2f9bc22f6a6926b254082";

            //List<CaptchaData> ssf = getCaptchaList();
            //var gresponse = "na";
            //if (ssf.Count > 0)
            //{
            //    gresponse = ssf[0].gresponse;
            //    deletecaptcha(gresponse);
            //}
            //else
            //{
            //    gresponse = Bypasscaptcha(sitekey, apiKey);
            //    if (gresponse == "NA")
            //    {
            //        //System.Threading.Thread.Sleep(3000);
            //        do
            //        {
            //            gresponse = Bypasscaptcha(sitekey, apiKey);
            //        } while (gresponse == "NA");
            //    }

            //}

            //Add Delivery Info
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3000));

            //ProfileSection s = new ProfileSection();
            List<ProfileData> ss = getprofileList(pr);

            //System.Threading.Thread.Sleep(3000);
            try
            {
                wait.Until(ExpectedConditions.ElementExists(By.Id("ContactInfo_text_firstName")));
                driver.FindElement(By.Id("ContactInfo_text_firstName")).SendKeys(ss[0].firstname);
                driver.FindElement(By.Id("ContactInfo_text_lastName")).SendKeys(ss[0].lastname);
                driver.FindElement(By.Id("ContactInfo_email_email")).SendKeys(ss[0].email);
                driver.FindElement(By.Id("ContactInfo_tel_phone")).SendKeys(ss[0].phone);
                wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("c-loading-curtain")));
                //wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("ReactModal_Overlay")));
                driver.FindElement(By.XPath("//*[@id='ContactInfo']/div[6]/button")).Click();
            }
            catch { }
            wait.Until(ExpectedConditions.ElementExists(By.Id("ShippingAddress_text_line1")));
            driver.FindElement(By.Id("ShippingAddress_text_line1")).SendKeys(ss[0].address1);

            driver.FindElement(By.Id("ShippingAddress_text_line2")).SendKeys(ss[0].apt);

            driver.FindElement(By.Id("ShippingAddress_text_postalCode")).SendKeys(ss[0].zipcode);

            driver.FindElement(By.Id("ShippingAddress_text_town")).SendKeys(ss[0].city);

            //driver.FindElement(By.Id("ShippingAddress_select_region")).Clear();
            driver.FindElement(By.Id("ShippingAddress_select_region")).SendKeys(ss[0].state);
            wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("c-loading-curtain")));
            //wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.ClassName("ReactModal_Overlay")));

            wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='step2']/div[2]/div[2]/button")));
            driver.FindElement(By.XPath("//*[@id='step2']/div[2]/div[2]/button")).Click();
            bw.ReportProgress(90);

            var month = ss[0].expiry.Split('/')[0].ToString().Trim();
            var year = ss[0].expiry.Split('/')[1].ToString().Trim();


            wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='adyenContainer']/div/div[1]/div[1]/span/iframe")));
            enterCreditCardNumber(ss[0].cardnumber, driver);
            driver.SwitchTo().DefaultContent();

            wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='adyenContainer']/div/div[1]/div[2]/div[1]/span/iframe")));
             driver.SwitchTo().Frame(driver.FindElement(By.XPath("//*[@id='adyenContainer']/div/div[1]/div[2]/div[1]/span/iframe")));
            driver.FindElement(By.Id("encryptedExpiryMonth")).SendKeys(month);
            driver.SwitchTo().DefaultContent();

            wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='adyenContainer']/div/div[1]/div[2]/div[2]/span/iframe")));
             driver.SwitchTo().Frame(driver.FindElement(By.XPath("//*[@id='adyenContainer']/div/div[1]/div[2]/div[2]/span/iframe")));
            driver.FindElement(By.Id("encryptedExpiryYear")).SendKeys(year);
            driver.SwitchTo().DefaultContent();

            wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='adyenContainer']/div/div[1]/div[2]/div[3]/span/iframe")));
            driver.SwitchTo().Frame(driver.FindElement(By.XPath("//*[@id='adyenContainer']/div/div[1]/div[2]/div[3]/span/iframe")));
            driver.FindElement(By.Id("encryptedSecurityCode")).SendKeys(ss[0].cvv);
            driver.SwitchTo().DefaultContent();
            //wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id='main']/div/div[2]/div/div/div/div[2]/div/div[2]/button")));
            driver.FindElement(By.XPath("//*[@id='main']/div/div[2]/div/div/div/div[2]/div/div[2]/button")).Click();
            

            wait.Until(ExpectedConditions.ElementExists(By.ClassName("Alert--error")));
            //driver.FindElement(By.Name("commit")).Click();
            var result = driver.FindElement(By.ClassName("Alert--error")).Text;
           driver.Quit();
            return result;


        }

        public static void enterCreditCardNumber(string cardNumber, IWebDriver driver)
        {
            System.Threading.Thread.Sleep(3000);
            driver.SwitchTo().Frame(driver.FindElement(By.XPath("//*[@id='adyenContainer']/div/div[1]/div[1]/span/iframe")));
            for (int i = 0; i < cardNumber.Length; i++)
            {
                char c = cardNumber[i];
                String s = new StringBuilder().Append(c).ToString();
                try
                {
                    driver.FindElement(By.Id("encryptedCardNumber")).SendKeys(s);
                }
                catch { }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using Google.Cloud.Speech.V1;
using System.Windows.Media.Imaging;
using static Dashboard1.ProfileSection;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Chrome;
using PuppeteerSharp;
using System.Net;
using System.Speech.Recognition;
using System.Text;
using System.IO;
using NAudio.Wave;

namespace Dashboard1.Bots
{
    class Supreme
    {
        IWebDriver driver;
        string global_sz = "";
        string global_pr = "";
        string global_url = "";
        string global_proxy = "";
        int retryCounter = 0;
        public BitmapImage fetchImage(IWebDriver driver)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20000));
            wait.Until(ExpectedConditions.ElementIsVisible(By.Id("img-main")));
            var img = driver.FindElement(By.Id("img-main")).GetAttribute("src");
            return new BitmapImage(new Uri(img));

        }

        public void initiateDriver(string proxy = "")
        {
            var po = proxy.Split('|')[0];
            Proxy p = new Proxy();


            var service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            var useragents = new List<string> { "Mozilla/5.0 (Windows NT 4.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/37.0.2049.0 Safari/537.36", "Mozilla/5.0 (X11; Linux x86_64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/33.0.1750.517 Safari/537.36", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_8_2) AppleWebKit/537.17 (KHTML, like Gecko) Chrome/24.0.1309.0 Safari/537.17" };
            int index = new Random().Next(useragents.Count);
            var name = useragents[index];
            useragents.RemoveAt(index);
            ChromeOptions option = new ChromeOptions();
            p.Kind = ProxyKind.Manual;
            p.IsAutoDetect = false;
            var tpr = proxy.Replace("|","").Split(':');
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
            else {
                p.SslProxy = proxy;
            }
            option.Proxy = p;
            //option.AddArgument("--ignore-certificate-errors");
            option.AddArguments("--window-size=1024,728");
            //option.AddArguments("--user-data-dir=C:/Users/intel/AppData/Local/Google/Chrome/User Data");

            //option.AddArguments("--user-agent='" + name + "'");
            //option.AddArguments("--headless");
            //option.PageLoadStrategy = PageLoadStrategy.Normal;
            if (po == "NA" || po == "")
            {

            }
            else
            {
                option.Proxy = p;

            }
            driver = new ChromeDriver(service, option);
            //driver.Manage().Window.Maximize();
        }

        public string FindProduct1(string sz, string pr, string url, string keyword, string proxy = "")
        {
            initiateDriver(proxy);
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2000));

            driver.Url = url;
            //keyword = keyword.ToUpper();
            //wait.Until(ExpectedConditions.ElementExists(By.PartialLinkText(keyword)));

            try
            {
                //Trefoil Essentials T-Shirt
                var name = keyword;// char.ToUpper(keyword[0]) + keyword.ToLower().Substring(1);
                                                 // wait.Until(ExpectedConditions.ElementExists(By.XPath("//img[contains(lower-case(@alt), '" + name + "')]")));
                                             //wait.Until(ExpectedConditions.ElementExists(By.XPath("//a[contains(@href, '" + sd + "')]/following-sibling::img[1]")));
                var prName = driver.FindElement(By.PartialLinkText(name)).GetAttribute("href");
                //var image = driver.FindElement(By.Id("img-main")).GetAttribute("src");
                //driver.Close();
                //driver.Dispose();
                //driver.Quit();
                return  "NA|" + prName;
            }
            catch (Exception ex)
            {
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
            global_sz = sz;
            global_pr = pr;
            global_url = url;
            global_proxy = proxy;
            initiateDriver(proxy);
            //driver = new ChromeDriver();
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2000));
            if (driver != null)
            {
                driver.Url = url;
            }
            else
            {

            }
            var tpr = proxy.Replace("|","").Split(':');
            if(tpr.Length > 2)
            {
                var username = tpr[2];
                var password = tpr[3];

            }
            try
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(By.Name("commit")));
                driver.FindElement(By.Name("commit")).Click();
            }
            catch
            {

            }

            try {
            wait.Until(ExpectedConditions.ElementToBeClickable(By.ClassName("checkout")));
            driver.FindElement(By.ClassName("checkout")).Click();
            }
            catch
            {

            }
            //Add Delivery Info
            try
            {
                
                return fillDelivery(driver, pr);
            }
            catch (Exception ex)
            {
                driver.Quit();
                return ex.Message;
            }

        }
        public string fillDelivery(IWebDriver driver, string pr)
        {
            //Add Delivery Info
            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20000));

            //ProfileSection s = new ProfileSection();
            List<ProfileData> ss = getprofileList(pr);

            System.Threading.Thread.Sleep(3000);
            try { 
            wait.Until(ExpectedConditions.ElementExists(By.Id("order_billing_name")));
            driver.FindElement(By.Id("order_billing_name")).SendKeys(ss[0].firstname + " " + ss[0].lastname);
            }
            catch { }
            driver.FindElement(By.Id("order_email")).SendKeys(ss[0].email);

              driver.FindElement(By.Id("order_tel")).SendKeys(ss[0].phone);
 driver.FindElement(By.Id("order_billing_zip")).SendKeys(ss[0].zipcode);

             driver.FindElement(By.Id("order_billing_state")).SendKeys(ss[0].state);

             driver.FindElement(By.Id("order_billing_city")).Clear();
            driver.FindElement(By.Id("order_billing_city")).SendKeys(ss[0].city);

             try
            {
                driver.FindElement(By.Id("bo")).SendKeys(ss[0].address1);
            }
            catch { }
            try
            {
                driver.FindElement(By.Id("order_billing_address")).SendKeys(ss[0].address1);
            }
            catch { }
              try
            {
                driver.FindElement(By.Id("oba3")).SendKeys(ss[0].apt);
            }
            catch { }
             enterCreditCardNumber(ss[0].cardnumber);
            driver.FindElement(By.Id("credit_card_month")).SendKeys(ss[0].expiry.Split('/')[0]);

            driver.FindElement(By.Id("credit_card_year")).SendKeys(ss[0].expiry.Split('/')[1]);

             try { 
            driver.FindElement(By.Id("orcer")).SendKeys(ss[0].cvv);
            }
            catch { }
            try { 
            driver.FindElement(By.Id("vval")).SendKeys(ss[0].cvv);
            }
            catch { }

              try {
            driver.FindElement(By.XPath("//*[@id='cart-cc']/fieldset/p/label/div/ins")).Click();
            }
            catch { }
            try {
            driver.FindElement(By.Id("order_terms")).Click();
            }
            catch { }
            try { 
                driver.FindElement(By.Name("commit")).Click();
            }
            catch { }
            try
            {
                driver.FindElement(By.Name("checkout")).Click();
            }
            catch { }
            //wait.Until(ExpectedConditions.FrameToBeAvailableAndSwitchToIt(By.XPath("//iframe[starts-with(@name, 'a-') and starts-with(@src, 'https://www.google.com/recaptcha')]")));
            //var s2 = driver.FindElement(By.XPath("//iframe[starts-with(@name, 'a-') and starts-with(@src, 'https://www.google.com/recaptcha')]")).Text;

            try {
                System.Threading.Thread.Sleep(3000);
                driver.SwitchTo().Frame(driver.FindElement(By.XPath("/html/body/div[3]/div[2]/iframe")));
            }
            catch { }

            try
            {
                System.Threading.Thread.Sleep(3000);
                driver.SwitchTo().Frame(driver.FindElement(By.XPath("/html/body/div[2]/div[2]/iframe")));
            }
            catch { }
            

            try
            {
                wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("recaptcha-audio-button")));
                driver.FindElement(By.Id("recaptcha-audio-button")).Click();
                var text1 = getCaptchaText();
                System.Threading.Thread.Sleep(3000);
                driver.FindElement(By.Id("audio-response")).SendKeys(text1);
                driver.FindElement(By.Id("recaptcha-verify-button")).Click();

                var error = driver.FindElement(By.ClassName("rc-audiochallenge-error-message")).Text;
                int count = 0;
                do
                {
                    count++;
                    System.Threading.Thread.Sleep(8000);
                    var text = getCaptchaText(count.ToString());
                    System.Threading.Thread.Sleep(3000);
                    driver.FindElement(By.Id("audio-response")).SendKeys(text);
                    driver.FindElement(By.Id("recaptcha-verify-button")).Click();
                    error = driver.FindElement(By.ClassName("rc-audiochallenge-error-message")).Text;
                } while (error != "");
                var result = driver.FindElement(By.XPath("//*[@id='confirmation']/p[1]")).Text;
                driver.Quit();
                return result;
            }
            catch (Exception ex)
            {
                //StartCheckout(global_sz, global_pr, global_url, global_proxy);
                var result = driver.FindElement(By.XPath("/html/body/div/div/div[1]/div[2]/div")).Text;
                driver.Quit();
                return result;

            }

        }
        private void configureAuth(String url, String username, String password)
        {
            driver.Url = "chrome-extension://enhldmjbphoeibbpdhmjkchohnidgnah/options.html";
            driver.FindElement(By.Id("url")).SendKeys(url);
            driver.FindElement(By.Id("username")).SendKeys(username);
            driver.FindElement(By.Id("password")).SendKeys(password);
            driver.FindElement(By.ClassName("credential-form-submit")).Click();
        }
        public void enterCreditCardNumber(string cardNumber)
        {
             for (int i = 0; i < cardNumber.Length; i++)
            {
                char c = cardNumber[i];
                String s = new StringBuilder().Append(c).ToString();
                try {
                driver.FindElement(By.Id("rnsnckrn")).SendKeys(s);
                }
                catch { }
                try
                {
                    driver.FindElement(By.Id("cnb")).SendKeys(s);
                }
                catch { }
            }
        }

        public string getCaptchaText(string co = "")
        {
            //var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20000));

            System.Threading.Thread.Sleep(3000);
            // wait.Until(ExpectedConditions.ElementExists(By.Id("audio-source")));
            var asource = driver.FindElement(By.Id("audio-source")).GetAttribute("src");

            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadFile(asource, @"Data/captcha" + co + ".mp3");
            }

            return getPassCode("Data/captcha" + co + ".mp3");
        }

        public string ConvertAudioToText(string path)
        {
            var speech = SpeechClient.Create();
            var config = new RecognitionConfig
            {
                Encoding = RecognitionConfig.Types.AudioEncoding.Flac,
                SampleRateHertz = 16000,
                LanguageCode = LanguageCodes.English.UnitedStates
            };
            var audio = RecognitionAudio.FromFile(path);

            var response = speech.Recognize(config, audio);

            var sd = "";
            foreach (var result in response.Results)
            {
                foreach (var alternative in result.Alternatives)
                {
                    sd += alternative.Transcript;
                    //Console.WriteLine(alternative.Transcript);
                }
            }
            return sd;
        }

        public string getPassCode(string path)
        {
            ConvertMp3ToWav(path, path.Replace(".mp3", ".wav"));
            SpeechRecognitionEngine sre = new SpeechRecognitionEngine();
            Grammar gr = new DictationGrammar();
            sre.LoadGrammar(gr);

            sre.SetInputToWaveFile(path.Replace(".mp3", ".wav"));
            sre.BabbleTimeout = new TimeSpan(Int32.MaxValue);
            sre.InitialSilenceTimeout = new TimeSpan(Int32.MaxValue);
            sre.EndSilenceTimeout = new TimeSpan(100000000);
            sre.EndSilenceTimeoutAmbiguous = new TimeSpan(100000000);

            StringBuilder sb = new StringBuilder();
            while (true)
            {
                try
                {
                    var recText = sre.Recognize();
                    if (recText == null)
                    {
                        break;
                    }

                    sb.Append(recText.Text);
                }
                catch (Exception ex)
                {
                    //handle exception      
                    //...

                    break;
                }
            }
            return sb.ToString();
        }

        private static void ConvertMp3ToWav(string _inPath_, string _outPath_)
        {
            using (Mp3FileReader mp3 = new Mp3FileReader(_inPath_))
            {
                using (WaveStream pcm = WaveFormatConversionStream.CreatePcmStream(mp3))
                {
                    WaveFileWriter.CreateWaveFile(_outPath_, pcm);
                }
            }
        }


        private bool IsElementPresent(By by)
        {
            try
            {
                System.Threading.Thread.Sleep(3000);
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
            this.driver.Close();
            this.driver.Quit();
        }
    }
}
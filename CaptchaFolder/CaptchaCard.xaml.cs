using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dashboard1.CaptchaFolder
{
    /// <summary>
    /// Interaction logic for CaptchaCard.xaml
    /// </summary>
    public partial class CaptchaCard : UserControl
    {
        IWebDriver driver;
        public CaptchaCard(string name="", string proxy = "", string date="")
        {
            InitializeComponent();
            delete_captcha.ToolTip = name;
            captcha_name.Content = name;
            if(proxy.Trim() == "") { 
                txtProxy.Content = "No Proxy";
            }
            else{
                txtProxy.Content = proxy;
            }
        }

        private void btnYouTube_Click(object sender, RoutedEventArgs e)
        {
            var service = ChromeDriverService.CreateDefaultService();
            ChromeOptions option = new ChromeOptions();
            service.HideCommandPromptWindow = true;
            option.AddArguments("--window-size=800,528");
            driver = new ChromeDriver(service, option);
            var url = "https://accounts.google.com/servicelogin?service=youtube";
            driver.Url = url;
        }
    }
}

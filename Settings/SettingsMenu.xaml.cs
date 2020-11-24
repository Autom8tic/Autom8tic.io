using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace Dashboard1.Settings
{
    /// <summary>
    /// Interaction logic for SettingsMenu.xaml
    /// </summary>
    public partial class SettingsMenu : Page
    {
        private ScriptHandler scriptHandler;
        public SettingsMenu()
        {
            InitializeComponent();
            this.scriptHandler = new ScriptHandler(this.PopUp);
            this.webBrowser.ObjectForScripting = this.scriptHandler;
        }

        public void MyMessageBox(string content, Visibility ok, Visibility cancel, Visibility close, string part)
        {
            MyMB MessageBox = new MyMB(content, ok, cancel, close, part);
            MessageBox.Show();
        }

        private void cancelSettings_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Cancel Action", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                this.NavigationService.Navigate(new Dashboard1.Dashboard.Dashboardndex());
            }
            //settings.Visibility = Visibility.Hidden;
            //settingsInfo.Visibility = Visibility.Hidden;
            //dashboard.Visibility = Visibility.Hidden;
            //dashboardinfo.Visibility = Visibility.Hidden;
        }
        private void Close_btnPaypal_Click(object sender, RoutedEventArgs e)
        {
            this.PopUp.IsOpen = false;

        }

        public void HideScriptErrors(WebBrowser wb, bool hide)
        {
            var fiComWebBrowser = typeof(WebBrowser).GetField("_axIWebBrowser2", BindingFlags.Instance | BindingFlags.NonPublic);
            if (fiComWebBrowser == null) return;
            var objComWebBrowser = fiComWebBrowser.GetValue(wb);
            if (objComWebBrowser == null)
            {
                wb.Loaded += (o, s) => HideScriptErrors(wb, hide); //In case we are to early
                return;
            }
            objComWebBrowser.GetType().InvokeMember("Silent", BindingFlags.SetProperty, null, objComWebBrowser, new object[] { hide });
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Popup_Configuration.IsOpen = false;
        }

        private void OpenCon_Click(object sender, RoutedEventArgs e)
        {
            Popup_Configuration.IsOpen = true;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Popup_Configuration.IsOpen = false;
        }

        private void saveSettings_Click(object sender, RoutedEventArgs e)
        {
            MyMessageBox("Settings Saved!!", Visibility.Hidden, Visibility.Hidden, Visibility.Visible, "");
            //MessageBox.Show("Settings Saved!!");
        }

        private void btn_paypalLogin_Click(object sender, RoutedEventArgs e)
        {
            //this.webBrowser.Navigate("https://www.paypal.com/in/signin");
            this.PopUp.IsOpen = true;
            HideScriptErrors(this.webBrowser, true);
        }
        private void btnshippingmanager_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Dashboard1.Settings.ShipManager());
            //settings.Visibility = Visibility.Hidden;
            //settingsInfo.Visibility = Visibility.Hidden;

            //shippingManager.Visibility = Visibility.Visible;
            //shippingManagerInfo.Visibility = Visibility.Visible;
        }

    }
}

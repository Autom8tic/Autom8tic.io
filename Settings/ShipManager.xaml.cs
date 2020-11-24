using System;
using System.Collections.Generic;
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

namespace Dashboard1.Settings
{
    /// <summary>
    /// Interaction logic for ShipManager.xaml
    /// </summary>
    public partial class ShipManager : Page
    {
        public ShipManager()
        {
            InitializeComponent();
        }

        private void open_shippingmanager(object sender, RoutedEventArgs e)
        {
            Popup_ShippingManger.IsOpen = true;
        }

        private void btn_settings_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new SettingsMenu());
            //MyMessageBox("Test Content", Visibility.Hidden, Visibility.Hidden, Visibility.Visible, "");


            //dashboard.Visibility = Visibility.Hidden;
            //dashboardinfo.Visibility = Visibility.Hidden;
            //cp.createProfile.Visibility = Visibility.Hidden;
            //cp.createprofileinfo.Visibility = Visibility.Hidden;
            //proxies1.Visibility = Visibility.Hidden;
            //proxiesinfo.Visibility = Visibility.Hidden;
            //proxiestester.Visibility = Visibility.Hidden;
            //proxiestesterinfo.Visibility = Visibility.Hidden;
            //captcha.Visibility = Visibility.Visible;
            //captchaprofileinfo.Visibility = Visibility.Visible;
            //createAccount.Visibility = Visibility.Hidden;
            //createAccountinfo.Visibility = Visibility.Hidden;
            //captchaprofileinfo.Visibility = Visibility.Visible;
            //openSolver.Visibility = Visibility.Hidden;
            //openSolverInfo.Visibility = Visibility.Hidden;
            //settings.Visibility = Visibility.Visible;
            //settingsInfo.Visibility = Visibility.Visible; 
            //shippingManager.Visibility = Visibility.Hidden;
            //shippingManagerInfo.Visibility = Visibility.Hidden;
            //releaseInfo.Visibility = Visibility.Hidden;
            //adproinfo.AdditonalProfile.Visibility = Visibility.Hidden;
            //adproinfo.GridAdditionalProfileInfo.Visibility = Visibility.Hidden;
            //releaseProfile.Visibility = Visibility.Hidden;
            //releaseinfo.Visibility = Visibility.Hidden;

        }

        private void btn_ShippingPopupClose(object sender, RoutedEventArgs e)
        {
            Popup_ShippingManger.IsOpen = false;
        }

    }
}

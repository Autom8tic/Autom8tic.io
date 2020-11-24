using Dashboard1.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dashboard1.Captchas
{
    /// <summary>
    /// Interaction logic for AddCaptchaInfo.xaml
    /// </summary>
    public partial class AddCaptchaInfo : Page
    {
        public AddCaptchaInfo()
        {
            InitializeComponent();

            if (getCaptchaData() > 0)
            {
                ButtonAutomationPeer peer = new ButtonAutomationPeer(btn_Captchas);
                IInvokeProvider invokeProv = peer.GetPattern(PatternInterface.Invoke) as IInvokeProvider;
                invokeProv.Invoke();
            }
        }
        public int getCaptchaData()
        {
            var filepath = "Data/ProfileData/captcha.csv";
            if (File.Exists(filepath))
            {
                var count = 0;
                using (var reader = new StreamReader(filepath))
                {
                    List<string> listA = new List<string>();
                    List<string> listB = new List<string>();
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        if (line == "")
                        {
                        }
                        else
                        {
                            count++;
                        }
                    }
                    reader.Dispose();
                }
                //this.NavigationService.Navigate(new CreateProfileCaptcha());
                return count;
            }
            else
            {
                return 0;
            }
        }
        private void btn_openSolver(object sender, RoutedEventArgs e)
        {
           
            this.NavigationService.Navigate(new CreateProfileCaptcha());

            //openSolver.Visibility = Visibility.Visible;
            //openSolverInfo.Visibility = Visibility.Visible;
        }

        private void btnaddAccount_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new CreateAccount());
            //captcha.Visibility = Visibility.Hidden;
            //captchainfo.Visibility = Visibility.Hidden;

            //createAccount.Visibility = Visibility.Visible;
            //createAccountinfo.Visibility = Visibility.Visible;
        }

        private void btn_Captchas_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new ShowCaptchaProfile());
        }
    }
}

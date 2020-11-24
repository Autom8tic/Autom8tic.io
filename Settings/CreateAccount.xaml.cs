using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for CreateAccount.xaml
    /// </summary>
    public partial class CreateAccount : Page
    {
        public CreateAccount()
        {
            InitializeComponent();
        }
        private void cancelAccount_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Dashboard1.Captchas.AddCaptchaInfo());
            //createAccount.Visibility = Visibility.Hidden;
            //createAccountinfo.Visibility = Visibility.Hidden;
            //captcha.Visibility = Visibility.Visible;
            //captchainfo.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (account_name.Text.Trim() == "")
            {
                account_name.BorderBrush = new SolidColorBrush(Colors.Red);
                return;
            }
            if (saveCaptcha()) {
                this.NavigationService.Navigate(new Dashboard1.Captchas.ShowCaptchaProfile());
            }
        }

        public bool saveCaptcha()
        {
            var filepath = "Data/ProfileData/captcha.csv";
            if (!System.IO.Directory.Exists("Data/ProfileData"))
                System.IO.Directory.CreateDirectory("Data/ProfileData");//.Dispose();
            if (!System.IO.File.Exists(filepath))
                System.IO.File.Create(filepath).Close();//.Dispose();
            bool result = false;

            using (StreamReader sr = new StreamReader(filepath))
            {
                CreateProfile p = new CreateProfile(null);
                if (!p.CheckExisting("captcha.csv", account_name.Text))
                {
                    sr.Dispose();
                    var csv = new StringBuilder();

                    //in your loop
                    var newLine = string.Format("{0},{1},{2}",
                        account_name.Text, proxy.Text.Trim(), DateTime.Now.ToString()
                        );
                    csv.AppendLine(newLine);
                    //after your loop
                    File.AppendAllText(filepath, csv.ToString());
                    result = true;
                }
                else
                {
                    MessageBox.Show("Release Already Exists..");
                    result = false;
                }
            }
            return result;
        }
    }
}

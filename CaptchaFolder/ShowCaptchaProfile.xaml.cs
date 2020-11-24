using Dashboard1.Settings;
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

namespace Dashboard1.Captchas
{
    /// <summary>
    /// Interaction logic for ShowCaptchaProfile.xaml
    /// </summary>
    public partial class ShowCaptchaProfile : Page
    {
        public ShowCaptchaProfile()
        {
            InitializeComponent();
            getCaptchaData();
        }

        public void getCaptchaData()
        {
            captcha_lst.Children.Clear();
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
                            CaptchaFolder.CaptchaCard pc = new CaptchaFolder.CaptchaCard(values[0], values[1], values[2]);
                            captcha_lst.Children.Add(pc);
                            Image grd = pc.FindName("delete_captcha") as Image;
                            grd.Cursor = Cursors.Hand;
                            grd.PreviewMouseDown += Pc_PreviewMouseDown;
                        }
                    }
                    reader.Dispose();
                }
                if (count > 0)
                {
                    captchainfo.Visibility = Visibility.Hidden;
                    captchaprofileinfo.Visibility = Visibility.Visible;
                }
                else
                {
                    captchainfo.Visibility = Visibility.Visible;
                    captchaprofileinfo.Visibility = Visibility.Hidden;
                }
            }
            else
            {
            }
        }

        private void Pc_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Image u = sender as Image;
                var filepath = "Data/ProfileData/captcha.csv";
                if (File.Exists(filepath))
                {
                    using (StreamReader sr = new StreamReader(filepath))
                    {
                        //foreach (string test in File.ReadLines(filepath))
                        //{
                        String line;
                        var count = 0;
                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] parts = line.Split(',');
                            if (parts[0].Trim() == u.ToolTip.ToString().Trim())
                            {
                                sr.Dispose();
                                string text = File.ReadAllText(filepath);
                                text = text.Replace(line, "").Remove(count, 1);
                                File.WriteAllText(filepath, text);
                                getCaptchaData();
                                break;
                            }
                            count++;
                        }
                        //}
                    }
                }
                else
                {

                }
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

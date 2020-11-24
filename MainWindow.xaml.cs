using Dashboard1.Captchas;
using Dashboard1.Proxies;
using Dashboard1.Dashboard;
using Dashboard1.Settings;
using Dashboard1.Release;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
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
using System.IO;
using Dashboard1.Bots;
using System.Speech.Recognition;
using NAudio.Wave;
using Dashboard1.Licence;
using System.ComponentModel;

namespace Dashboard1
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>

    public partial class MainWindow : Window
    {
        
        ProfileSection ps = new ProfileSection();
        CreateProfile cp = new CreateProfile("");
        AdditionalProfileInfo adproinfo = new AdditionalProfileInfo(null,null);
        
        List<ProfileData> current_data = new List<ProfileData>();
        public ObservableCollection<ComboBoxItem> cbItems { get; set; }
        public ComboBoxItem SelectedcbItem { get; set; }
        public class ProfileData
        {
            public string name { get; set; }
            public string Country { get; set; }
            
        }
        public void MyMessageBox(string content, Visibility ok, Visibility cancel, Visibility close, string part)
        {
            MyMB MessageBox = new MyMB(content, ok, cancel, close, part);
            MessageBox.Show();
        }

        public void profileView()
        {
            frame.NavigationService.Navigate(new ProfileSection());
        }
        public void hideFrame()
        {
            frame.Visibility = Visibility.Hidden;
        }
        bool is_sec_page = false;
        mshtml.HTMLDocument htmldoc;

        //WebBrowser webBrowser1 = new WebBrowser();
        public MainWindow()
        {
            var result = ValidateLicence.validateLicnece();
            if (result.Contains("Valid:1"))
            {
                InitializeComponent();
                Consumo consumo = new Consumo();
                DataContext = new ConsumoViewModel(consumo);

                AdditionalProfileInfo adproinfo = new AdditionalProfileInfo(null, null);
                adproinfo.RefreshProfiles();
                frame.Visibility = Visibility.Visible;
                frame.NavigationService.Navigate(new Dashboardndex());
            }
            else
            {
                if (result.Contains("network-related"))
                {
                    this.Hide();
                    MessageBox.Show("Please Check Your Internet Connection.");
                    return;
                }
                this.Hide();
                Licence.Licence win2 = new Dashboard1.Licence.Licence();
                win2.Activate();
                win2.Show();
            }
           
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

        public string getPassCode(string path)
        {
            ConvertMp3ToWav(path, "Data/");
            SpeechRecognitionEngine sre = new SpeechRecognitionEngine();
            Grammar gr = new DictationGrammar();
            sre.LoadGrammar(gr);

            sre.SetInputToWaveFile(path);
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


        private void Navigate_Click(object sender, RoutedEventArgs e)//By Prince Jain 
        {
            Captcha c = new Captcha();
            c.Show();
            c.Activate();
        }

        public void checkLicence()
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += Bw_CheckLicence;
            bw.RunWorkerCompleted += Bw_CheckLicenceCompleted;
            bw.WorkerSupportsCancellation = true;
            //Parameter you need to work in Background-Thread for example your strings
            string[] param = new[] {"" };

            //Start work
            bw.RunWorkerAsync(param);

        }

        private void Bw_CheckLicence(object sender, DoWorkEventArgs e)
        {
            string[] param = e.Argument as string[];

            var result = ValidateLicence.validateLicnece();
           
            e.Result = result;
        }
        private void Bw_CheckLicenceCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var result = e.Result as string;
            if (result.Contains("Valid:1"))
            {
                var days = result.Split('|')[1].Replace("Days:", "");
                txtLicence.Text = "The Licence has " + days.ToString() + " days left";
            }
            else
            {
                if (result.Contains("network-related"))
                {
                    this.Hide();
                    MessageBox.Show("Please Check Your Internet Connection.");
                    return;
                }
                this.Hide();
                Licence.Licence win2 = new Dashboard1.Licence.Licence();
                win2.Activate();
                win2.Show();
            }
        }


        private void btnRelease_Click(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new ReleaseList());
            btnRelease.Background = new SolidColorBrush(Colors.LightBlue);
            btnProfile.Background = new SolidColorBrush(Colors.Black);
            btnProxies.Background = new SolidColorBrush(Colors.Black);
            btnCaptchas.Background = new SolidColorBrush(Colors.Black);
            checkLicence();

        }


        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new Dashboardndex());
            checkLicence();
        }
        private void btnProfile_Click(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new ProfileSection());

            btnRelease.Background = new SolidColorBrush(Colors.Black);
            btnProfile.Background = new SolidColorBrush(Colors.LightBlue);
            btnProxies.Background = new SolidColorBrush(Colors.Black);
            btnCaptchas.Background = new SolidColorBrush(Colors.Black);
            checkLicence();
        }
        private void btnProxies_Click(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new AddProxiesInfo());
            btnRelease.Background = new SolidColorBrush(Colors.Black);
            btnProfile.Background = new SolidColorBrush(Colors.Black);
            btnProxies.Background = new SolidColorBrush(Colors.LightBlue);
            btnCaptchas.Background = new SolidColorBrush(Colors.Black);
            checkLicence();
        }
        private void btnCaptchas_Click(object sender, RoutedEventArgs e)
        {
            //frame.NavigationService.Navigate(new Dashboard1.Captchas.ShowCaptchaProfile());

            //btnRelease.Background = new SolidColorBrush(Colors.Black);
            //btnProfile.Background = new SolidColorBrush(Colors.Black);
            // btnProxies.Background = new SolidColorBrush(Colors.Black);
            // btnCaptchas.Background = new SolidColorBrush(Colors.LightBlue);
            MessageBox.Show("Auto Captcha is on for purchases. This Feature is under process");
            checkLicence();
        }
        private void btn_settings_Click(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new SettingsMenu());
            //MyMessageBox("Test Content", Visibility.Hidden, Visibility.Hidden, Visibility.Visible, "");

            checkLicence();
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
        
        ProfileData pf = new ProfileData();
        private void btn_profile_Click(object sender, RoutedEventArgs e)
        {
            if (cp.profile_name.Text.Trim() != "")
            {
                frame.NavigationService.Navigate(new CreateProfile(""));

                //dashboard.Visibility = Visibility.Hidden;
                //dashboardinfo.Visibility = Visibility.Hidden;
                //ps.profile.Visibility = Visibility.Hidden;
                //cp.createProfile.Visibility = Visibility.Hidden;
                //cp.createprofileinfo.Visibility = Visibility.Hidden;
                //ps.profileinfo.Visibility = Visibility.Hidden;
                //cp.createProfile2.Visibility = Visibility.Visible;
                //cp.createprofileinfo2.Visibility = Visibility.Visible;
                //proxies1.Visibility = Visibility.Hidden;
                //proxiesinfo.Visibility = Visibility.Hidden;
                //captcha.Visibility = Visibility.Hidden;
                //captchainfo.Visibility = Visibility.Hidden;
                pf.name = cp.profile_name.Text;
                
            }
            else
            {
                cp.profile_name.BorderBrush = new SolidColorBrush(Colors.Red);
            }
        }

        
        private void GridBarraTitulo_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void btn_showRelease(object sender, RoutedEventArgs e)
        {
            ReleasePanel rp = new ReleasePanel();
            //frame.NavigationService.Navigate(new ReleasePanel());
            rp.releaseInfo.Visibility = Visibility.Visible;

        }
        private void showRelease_Click(object sender, RoutedEventArgs e)
        {
            frame.NavigationService.Navigate(new CreateRelease());

        }
        
        private void checkforupdate_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://swxpe.io/");
        }
        private void ButtonFechar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void twitter_btn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://twitter.com/");
        }

        private void home_btn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://swxpe.io/");
        }

        private void email_btn_Click(object sender, RoutedEventArgs e)
        {

        }
        private void OnNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }

        private void CountryFlag_MouseEnter(object sender, MouseEventArgs e)
        {
            var item = sender as CountryFlag.CountryFlag;
            item.BorderBrush = new SolidColorBrush(Colors.Red);
        }

        
        public ImageBrush imageBinding
        {
            get
            {
                ImageBrush brush = new ImageBrush();

                var r = new RegionInfo(pf.Country);
                var flagName = r.TwoLetterISORegionName + ".jpg";

                //brush.ImageSource = new BitmapImage(new Uri("http://labs.ucaya.com/ucaya.png"));
                return brush;
            }
        }

        
        public class ComboData
        {
            public string Id { get; set; }
            public string Value { get; set; }
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
        
        
        private void saveSettings_Click(object sender, RoutedEventArgs e)
        {
            MyMessageBox("Settings Saved!!", Visibility.Hidden, Visibility.Hidden, Visibility.Visible, "");
            //MessageBox.Show("Settings Saved!!");
        }

        private void btn_minimize_Click(object sender, RoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }
    }

    internal class ConsumoViewModel
    {
        public List<Consumo> Consumo { get; private set; }

        public ConsumoViewModel(Consumo consumo)
        {
            Consumo = new List<Consumo>();
            Consumo.Add(consumo);
        }
    }



    internal class Consumo
    {

        public string Titulo { get; private set; }
        public int Porcentagem { get; private set; }

        public Consumo()
        {
            Titulo = "SWXPE.IO";
            Porcentagem = CalcularPorcentagem();
        }

        private int CalcularPorcentagem()
        {
            return 47; //Calculo da porcentagem de consumo
        }

    }
}

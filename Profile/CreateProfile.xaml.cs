using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
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
namespace Dashboard1
{
    /// <summary>
    /// Interaction logic for CreateProfile.xaml
    /// </summary>
    public partial class CreateProfile : Page
    {
        public class ProfileData
        {
            public string name { get; set; }
            public string Country { get; set; }

        }
        public CreateProfile(string profile)
        {
            InitializeComponent();
            if (profile == "profile")
            {
                this.NavigationService.Navigate(new ProfileSection());
            }
            //createProfile.Visibility = Visibility.Visible;
            createprofileinfo.Visibility = Visibility.Visible;
            PopulateCountryComboBox();

        }
        
        private void cancelProfile_Click(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            ProfileSection ps = new ProfileSection();
            AdditionalProfileInfo adproinfo = new AdditionalProfileInfo(null,null);
            CreateProfile cp = new CreateProfile("");


            //this.NavigationService.Navigate(new MyMB("", Visibility, Visibility, Visibility, ""));
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Cancel Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {

                this.NavigationService.Navigate(new ProfileSection());

                //ps.profile.Visibility = Visibility.Visible;
                //ps.profileinfo.Visibility = Visibility.Visible;
                //createProfile.Visibility = Visibility.Hidden;
                createprofileinfo.Visibility = Visibility.Hidden;
                //createProfile2.Visibility = Visibility.Hidden;
                //createprofileinfo2.Visibility = Visibility.Hidden;
                //adproinfo.AdditonalProfile.Visibility = Visibility.Hidden;
                adproinfo.GridAdditionalProfileInfo.Visibility = Visibility.Hidden;

                adproinfo.ClearAllTextboxProfile();

            }

            //main.MyMessageBox("Are you sure want to cancel?", Visibility.Visible, Visibility.Visible, Visibility.Hidden, "ProfileSection");
            // Change the page of the frame.
            //if (pageFrame != null)
            //{
            //    this.NavigationService.Navigate(new ProfileSection("profile"));
            //}
            //main.MyMessageBox("Are you sure?", Visibility.Visible, Visibility.Visible, Visibility.Hidden, "cancelProfile");

        }

        private void cancelImport_Click(object sender, RoutedEventArgs e)
        {
            ProfileSection ps = new ProfileSection();
           // importProfile.Visibility = Visibility.Hidden;
            //importprofileinfo.Visibility = Visibility.Hidden;
            //ps.profile.Visibility = Visibility.Visible;
            ps.profileinfo.Visibility = Visibility.Visible;
        }
        private void btn_profile_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ProfileData pf = new MainWindow.ProfileData();
            MainWindow main = new MainWindow();
            ProfileSection ps = new ProfileSection();

            if (CheckExisting("profiles.csv",profile_name.Text))
            {
                MessageBox.Show("Profile Name Already exists");
                profile_name.BorderBrush = new SolidColorBrush(Colors.Red);
                return;
            }
            if (profile_name.Text.Trim() != "")
            {
                this.NavigationService.Navigate(new AdditionalProfileInfo(profile_name.Text, country_listBox.SelectedValue.ToString()));
                //main.frame.Visibility = Visibility.Hidden;
                //main.dashboardinfo.Visibility = Visibility.Hidden;
                // ps.profile.Visibility = Visibility.Hidden;
                // createProfile.Visibility = Visibility.Hidden;
                //createprofileinfo.Visibility = Visibility.Hidden;
                ps.profileinfo.Visibility = Visibility.Hidden;
                //createProfile2.Visibility = Visibility.Visible;
                //createprofileinfo2.Visibility = Visibility.Visible;
                //main.proxies1.Visibility = Visibility.Hidden;
                //main.proxiesinfo.Visibility = Visibility.Hidden;
                //main.captcha.Visibility = Visibility.Hidden;
                //main.captchainfo.Visibility = Visibility.Hidden;
                pf.name = profile_name.Text;
                //Name = pf.name;
            }
            else
            {
                profile_name.BorderBrush = new SolidColorBrush(Colors.Red);
            }
        }

        public bool CheckExisting(string file, string name)
        {
           bool IsExists = false;
            var filepath = "Data/ProfileData/" + file;
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
                            if(values[0].Trim() == name.Trim())
                            {
                                IsExists = true;
                                break;
                            }
                        }
                    }
                    reader.Dispose();
                }
                if (count > 0)
                {
                }
            }

            return IsExists;
        }


        //public String Name
        //{
        //    get { return profile_name.Text; }
        //    set { profile_name.Text = value; }
        //}

        private void PopulateCountryComboBox()
        {
            RegionInfo country = new RegionInfo(new CultureInfo("en-US", false).LCID);
            List<string> countryNames = new List<string>();

            List<MainWindow.ComboData> ListData = new List<MainWindow.ComboData>();
            foreach (CultureInfo cul in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                country = new RegionInfo(new CultureInfo(cul.Name, false).LCID);

                //countryNames.Add(country.ThreeLetterISORegionName.ToString());

                MainWindow.ComboData c = new MainWindow.ComboData();
                c.Id = country.DisplayName;
                c.Value = country.ThreeLetterISORegionName;
                if (c.Value != "DJI")
                {
                    ListData.Add(c);
                }

            }

            IEnumerable<string> nameAdded = countryNames.OrderBy(names => names).Distinct();

            //foreach (string item in nameAdded)
            //{
            //    country_listBox.Items.Add(item);

            //}

            //country_listBox.SelectedIndex = 0;



            country_listBox.ItemsSource = ListData;
            country_listBox.DisplayMemberPath = "Id";
            country_listBox.SelectedValuePath = "Value";
            country_listBox.SelectedValue = "USA";

        }

        private void btn_showAdditionalInfo(object sender, RoutedEventArgs e)
        {
            MainWindow main = new MainWindow();
            ProfileSection ps = new ProfileSection();
            CreateProfile cp = new CreateProfile("");

            if (country_listBox.SelectedIndex == -1)
            {
                main.MyMessageBox("Please select Region", Visibility.Hidden, Visibility.Hidden, Visibility.Visible, "");

                //MessageBox.Show("Please select Region");
            }
            else
            {
                this.NavigationService.Navigate(new AdditionalProfileInfo(profile_name.Text, pf.Country));

               // ps.profile.Visibility = Visibility.Hidden;
                ps.profileinfo.Visibility = Visibility.Hidden;
               // createProfile.Visibility = Visibility.Hidden;
                createprofileinfo.Visibility = Visibility.Hidden;
               // createProfile2.Visibility = Visibility.Hidden;
               // createprofileinfo2.Visibility = Visibility.Hidden;
            }
        }

        private void fileUploadCLick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = ".txt"; // Required file extension 
            fileDialog.Filter = "Text documents (.txt)|*.txt"; // Optional file extensions

            fileDialog.ShowDialog();
        }
        ProfileData pf = new ProfileData();
        private void country_listBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ListBox lst = sender as ListBox;
                CountryFlag.CountryFlag code = (lst.SelectedItem as ListBoxItem).Content as CountryFlag.CountryFlag;
                pf.Country = code.Code.ToString();
            }
            catch (Exception ex)
            {

            }

            // MessageBox.Show(code.Code.ToString());
        }

        private void continueImport_Click(object sender, RoutedEventArgs e)
        {
            ProfileSection ps = new ProfileSection();
            CreateProfile cp = new CreateProfile("");

            //cp.importProfile.Visibility = Visibility.Hidden;
            //cp.importprofileinfo.Visibility = Visibility.Hidden;
            //ps.profile.Visibility = Visibility.Visible;
            ps.profileinfo.Visibility = Visibility.Visible;
        }


    }
}

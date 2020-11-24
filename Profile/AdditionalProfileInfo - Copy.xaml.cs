using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for AdditionalProfileInfo.xaml
    /// </summary>
    public partial class AdditionalProfileInfo : Page
    {
        List<CreateProfile.ProfileData> profile_list = new List<CreateProfile.ProfileData>();
        string pf_Name = "", pf_country = "";
        bool pf_IsEdit = false;
        public AdditionalProfileInfo(string Name, string Country, bool IsEdit = false)
        {
            InitializeComponent();

            //AdditonalProfile.Visibility = Visibility.Visible;
            GridAdditionalProfileInfo.Visibility = Visibility.Visible;
            pf_Name = Name;
            pf_country = Country;
            pf_IsEdit = IsEdit;
            PopulateCountryComboBox();
            if (IsEdit) {
                getProfileData();
            }

        }

        public void ClearAllTextboxProfile()
        {
            CreateProfile cp = new CreateProfile("");

            cp.profile_name.Text = string.Empty;
            //cp.country_listBox.SelectedIndex = -1;

            //grid Delivery Address
            TxtFNameDA.Text = string.Empty;
            TxtLNameDA.Text = string.Empty;
            TxtPhoneNoDA.Text = string.Empty;
            TxtAddressDA.Text = string.Empty;
            TxtAptDA.Text = string.Empty;
            TxtCityDA.Text = string.Empty;
            TxtStateDA.Text = string.Empty;
            TxtZipCodeDA.Text = string.Empty;
            TxtPhoneNoDA.Text = string.Empty;

            //grid Billing Address
            TxtFNameBA.Text = string.Empty;
            TxtLNameBA.Text = string.Empty;
            TxtPhoneNoBA.Text = string.Empty;
            TxtAddressBA.Text = string.Empty;
            TxtAptBA.Text = string.Empty;
            TxtCityBA.Text = string.Empty;
            cbStateBA.Text = string.Empty;
            TxtZipCodeBA.Text = string.Empty;
            TxtPhoneNoBA.Text = string.Empty;

            //grid Payment Details
            TxtCardHolderPD.Text = string.Empty;
            TxtCardNoPD.Text = string.Empty;
            TxtCvvPD.Text = string.Empty;
            TxtEmailPD.Text = string.Empty;
            TxtExpDatePD.Text = string.Empty;

            //grid Miscellaneous
           // cbStateMis.SelectedIndex = 0;
            ChDelAddressMis.IsChecked = false;
        }


        private void OnlyChar(object sender, KeyEventArgs e)
        {
            if (!(e.Key >= Key.A && e.Key <= Key.Z))
            {
                e.Handled = true;
            }
        }
        private void OnlyNumber(object sender, KeyEventArgs e)
        {
            if (e.Key >= Key.A && e.Key <= Key.Z)
            {
                e.Handled = true;
            }
        }

        public void saveProfileLocally()
        {
            var filepath = "Data/ProfileData/profiles.csv";
            if (!System.IO.Directory.Exists("Data/ProfileData"))
                System.IO.Directory.CreateDirectory("Data/ProfileData");//.Dispose();
            if (!System.IO.File.Exists(filepath))
                System.IO.File.Create(filepath).Close();//.Dispose();
            var firstName = TxtFNameDA.Text.Replace(",","|");
            var LastName = TxtLNameDA.Text.Replace(",", "|");
            var Address = TxtAddressDA.Text.Replace(",", "|");
            var AptDA = TxtAptDA.Text.Replace(",", "|");
            var CityDA = TxtCityDA.Text.Replace(",", "|");
            var StateDA = TxtStateDA.Text.Replace(",", "|");
            var ZipCodeDA = TxtZipCodeDA.Text.Replace(",", "|");
            var PhoneNoDA = TxtPhoneNoDA.Text.Replace(",", "|");
            
            var FNameBA = TxtFNameBA.Text.Replace(",", "|");
            var LNameBA = TxtLNameBA.Text.Replace(",", "|");
            var AddressBA = TxtAddressBA.Text.Replace(",", "|");
            var AptBA = TxtAptBA.Text.Replace(",", "|");
            var CityBA = TxtCityBA.Text.Replace(",", "|");
            var StateBA = cbStateBA.Text.Replace(",", "|");
            var ZipCodeBA = TxtZipCodeBA.Text.Replace(",", "|");
            var PhoneNoBA = TxtPhoneNoBA.Text.Replace(",", "|");
            
            var EmailAddress = TxtEmailPD.Text.Replace(",", "|");
            var CardHolder = TxtCardHolderPD.Text.Replace(",", "|");
            var CardNo = TxtCardNoPD.Text.Replace(",", "|");
            var ExpDate = TxtExpDatePD.Text.Replace(",", "|");
            

            var Cvv = TxtCvvPD.Text.Replace(",", "|");
            var AddressRegion = "";
            if (cbStateMis.SelectedValue != null) {
                AddressRegion = cbStateMis.SelectedValue.ToString().Replace(",", "|");
            }
            else
            {
                AddressRegion = null;
            }
            var BASameASDA = false;
            if (ChDelAddressMis.IsChecked == true) {
                BASameASDA = true;
                FNameBA = TxtFNameDA.Text.Replace(",", "|");
                LNameBA = TxtLNameDA.Text.Replace(",", "|");
                AddressBA = TxtAddressDA.Text.Replace(",", "|");
                AptBA = TxtAptDA.Text.Replace(",", "|");
                CityBA = TxtCityDA.Text.Replace(",", "|");
                StateBA = TxtStateDA.Text.Replace(",", "|");
                ZipCodeBA = TxtZipCodeDA.Text.Replace(",", "|");
                PhoneNoBA = TxtPhoneNoDA.Text.Replace(",", "|");
            }
            if (pf_IsEdit)
            {
                using (StreamReader sr = new StreamReader(filepath))
                {
              
                    //foreach (string test in File.ReadLines(filepath))
                    //{
                        String line;

                        while ((line = sr.ReadLine()) != null)
                        {
                            string[] parts = line.Split(',');
                            if (parts[0] == pf_Name)
                        {
                            sr.Dispose();
                            var newData = pf_Name + "," + pf_country + "," + firstName + "," + LastName + "," + Address + "," + AptDA + "," + CityDA + "," + StateDA + "," + ZipCodeDA + "," + PhoneNoDA + "," +
                  FNameBA + "," + LNameBA + "," + AddressBA + "," + AptBA + "," + CityBA + "," + StateBA + "," +
                  ZipCodeBA + "," + PhoneNoBA + "," + EmailAddress + "," + CardHolder + "," + CardNo + "," + ExpDate + "," +
                  Cvv + "," + AddressRegion + "," + line[24] + "," + DateTime.Now.ToString() + "," + BASameASDA;
                                string text = File.ReadAllText(filepath);
                                text = text.Replace(line, newData);
                                File.WriteAllText(filepath, text);
                                break;
                            }

                        }
                    //}
                }
            }
            else
            {
                //before your loop
                var csv = new StringBuilder();
                //in your loop
                var newLine = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26}",
                    pf_Name, pf_country, firstName, LastName, Address, AptDA, CityDA, StateDA, ZipCodeDA, PhoneNoDA
                    , FNameBA, LNameBA, AddressBA, AptBA, CityBA, StateBA, ZipCodeBA, PhoneNoBA
                    , EmailAddress, CardHolder, CardNo, ExpDate, Cvv, AddressRegion, DateTime.Now.ToString(), DateTime.Now.ToString(),BASameASDA
                    );
                csv.AppendLine(newLine);
                //after your loop
                File.AppendAllText(filepath, csv.ToString());
            }
        }

        public void getProfileData() {
            var filepath = "Data/ProfileData/profiles.csv";
            try {
            if (File.Exists(filepath))
            {
                var count = 0;
                using (var reader = new StreamReader(filepath))
                {
                    List<string> listA = new List<string>();
                    List<string> listB = new List<string>();
                    while (!reader.EndOfStream)
                    {
                        count++;
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        if (line == "")
                        {

                        }
                        else
                        {
                            if(values[0] == pf_Name)
                            {
                                TxtFNameDA.Text = values[2];
                                TxtLNameDA.Text = values[3];
                                TxtAddressDA.Text = values[4];
                                TxtAptDA.Text = values[5];
                                TxtCityDA.Text = values[6];
                                TxtStateDA.Text = values[7];
                                TxtZipCodeDA.Text = values[8];
                                TxtPhoneNoDA.Text = values[9];
                                if (values[26].ToLower() == "true") {
                                        ChDelAddressMis.IsChecked = true;
                                    DisableBA();
                                } else
                                {
                                    TxtFNameBA.Text = values[10];
                                    TxtLNameBA.Text = values[11];
                                    TxtAddressBA.Text = values[12];
                                    TxtAptBA.Text = values[13];
                                    TxtCityBA.Text = values[14];
                                    cbStateBA.Text = values[15];
                                    TxtZipCodeBA.Text = values[16];
                                    TxtPhoneNoBA.Text = values[17];
                                        ChDelAddressMis.IsChecked = false;
                                    }
                                
                                TxtEmailPD.Text = values[18];
                                TxtCardHolderPD.Text = values[19];
                                TxtCardNoPD.Text = values[20];
                                TxtExpDatePD.Text = values[21];
                                TxtCvvPD.Text = values[22];
                                cbStateMis.SelectedValue = values[23];
                            }
                        }
                    }
                }
                if (count > 0)
                {
                }
            }
            else
            {
            }
            }
            catch
            {

            }
        }

        private void btnSaveadditional_info_Click(object sender, RoutedEventArgs e)
        {
            CreateProfile.ProfileData pf = new CreateProfile.ProfileData();
            var ExpDate = TxtExpDatePD.Text.Replace(",", "|");
            var re = @"^(0[1-9]|1[0-2])\/?(([0-9]{4})$)";
            var result = Regex.Match(ExpDate, re);
            if (result.Success != true)
            {
                MessageBox.Show("ExpiryDate Pattern Should be like this: 05/2024");
                return;
            }

            saveProfileLocally();
            //ProfileSection ps = new ProfileSection();
            CreateProfile cp = new CreateProfile("");
            MainWindow main = new MainWindow();
            //cp.createProfile.Visibility = Visibility.Hidden;
            //cp.createprofileinfo.Visibility = Visibility.Hidden;
            //cp.createProfile2.Visibility = Visibility.Hidden;
            //cp.createprofileinfo2.Visibility = Visibility.Hidden;
            //AdditonalProfile.Visibility = Visibility.Hidden;
            //GridAdditionalProfileInfo.Visibility = Visibility.Hidden;

            this.NavigationService.Navigate(new ProfileSection());
            //ClearAllTextboxProfile();
            //; profile_list.Add(pf);
            //RefreshProfiles();

            //main.MyMessageBox("Profile saved", Visibility.Hidden, Visibility.Hidden, Visibility.Visible, "");  

            //MessageBox.Show("Profile saved");

        }

        private void CheckBoxcheck(object sender, RoutedEventArgs e)
        {
            DisableBA();
        }
        private void CheckBoxUncheck(object sender, RoutedEventArgs e)
        {
            EnableBA();
        }
        public void DisableBA()
        {
            TxtFNameBA.IsEnabled = false;
            TxtLNameBA.IsEnabled = false;
            TxtAddressBA.IsEnabled = false;
            TxtAptBA.IsEnabled = false;
            TxtCityBA.IsEnabled = false;
            cbStateBA.IsEnabled = false;
            TxtZipCodeBA.IsEnabled = false;
            TxtPhoneNoBA.IsEnabled = false;
        }

        public void EnableBA()
        {

            TxtFNameBA.IsEnabled = true;
            TxtLNameBA.IsEnabled = true;
            TxtAddressBA.IsEnabled = true;
            TxtAptBA.IsEnabled = true;
            TxtCityBA.IsEnabled = true;
            cbStateBA.IsEnabled = true;
            TxtZipCodeBA.IsEnabled = true;
            TxtPhoneNoBA.IsEnabled = true;
        }
        public void RefreshProfiles()
        {
            ProfileSection ps = new ProfileSection();

            ps.no_profile_panel.Visibility = Visibility.Hidden;
            ps.profile_panel.Visibility = Visibility.Visible;
            if (profile_list.Count > 0)
            {

                foreach (var item in profile_list)
                {
                    //pf_Name.Text = item.name;
                    // cpr.Code = item.Country as CountryFlag.CountryCode;
                    //wrap_profiles.Children.Add(sample_profile);
                }

            }
            else
            {
                ps.no_profile_panel.Visibility = Visibility.Visible;
                ps.profile_panel.Visibility = Visibility.Hidden;
            }
        }

        private void cancelProfile_Click(object sender, RoutedEventArgs e)
        {
            ProfileSection ps = new ProfileSection();
            CreateProfile cp = new CreateProfile("");

            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Cancel Confirmation", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                this.NavigationService.Navigate(new ProfileSection());

                //ps.profile.Visibility = Visibility.Visible;
                ps.profileinfo.Visibility = Visibility.Visible;
                //cp.createProfile.Visibility = Visibility.Hidden;
                cp.createprofileinfo.Visibility = Visibility.Hidden;
                //cp.createProfile2.Visibility = Visibility.Hidden;
                //cp.createprofileinfo2.Visibility = Visibility.Hidden;
                //AdditonalProfile.Visibility = Visibility.Hidden;
                GridAdditionalProfileInfo.Visibility = Visibility.Hidden;
                
                ClearAllTextboxProfile();
            }
        }
        private void clearProfile_Click(object sender, RoutedEventArgs e)
        {
            ClearAllTextboxProfile();
        }

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
                c.Value = country.TwoLetterISORegionName;
                if(c.Value != "DJ") {
                ListData.Add(c);
                }

            }

            IEnumerable<string> nameAdded = countryNames.OrderBy(names => names).Distinct();

            cbStateMis.ItemsSource = ListData;
            cbStateMis.DisplayMemberPath = "Id";
            cbStateMis.SelectedValuePath = "Value";
            cbStateMis.SelectedValue = "US";

        }

    }
}

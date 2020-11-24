using Dashboard1.Profile;
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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dashboard1
{
    /// <summary>
    /// Interaction logic for ProfileSection.xaml
    /// </summary>
    public partial class ProfileSection : Page
    {
        public class ProfileData
        {
            public string name { get; set; }
            public string Country { get; set; }
            public string firstname { get; set; }
            public string lastname { get; set; }
            public string address1 { get; set; }
            public string apt { get; set; }
            public string city { get; set; }
            public string state { get; set; }
            public string zipcode { get; set; }
            public string phone { get; set; }
            public string email { get; set; }
            public string cardholder { get; set; }
            public string cardnumber { get; set; }
            public string expiry { get; set; }
            public string cvv { get; set; }
            public string Billingregion { get; set; }

        }

        public ProfileSection()
        {
            InitializeComponent();
            //profile.Visibility = Visibility.Visible;
            profileinfo.Visibility = Visibility.Visible;
            getProfileData();
        }

        public void getProfileData() {
            var filepath = "Data/ProfileData/profiles.csv";
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
                            ProfileCard pc = new ProfileCard(values[0], values[1], values[24]);
                            wrap_profiles.Children.Add(pc);
                            Grid grd = pc.FindName("sample_profile") as Grid;
                            grd.PreviewMouseDown += Pc_PreviewMouseDown;
                        }
                    }
                    reader.Dispose();
                }
                if (count > 0)
                {
                    profile_panel.Visibility = Visibility.Visible;
                    no_profile_panel.Visibility = Visibility.Hidden;

                }
            }
            else
            {
            }
        }

        public static List<ProfileData> getprofileList(string pr = "") {
            var filepath = "Data/ProfileData/profiles.csv";
            List<ProfileData> ls = new List<ProfileData>();
            if (File.Exists(filepath))
            {
                var count = 0;
                using (var reader = new StreamReader(filepath))
                {
                    List<string> listA = new List<string>();
                    List<string> listB = new List<string>();
                    while (!reader.EndOfStream)
                    {
                        ProfileData obj = new ProfileData();
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        if (line == "")
                        {
                        }
                        else
                        {
                            if (pr == "") { 
                            count++;
                            obj.name = values[0];
                                obj.Country = values[1];
                                obj.firstname = values[2];
                            obj.lastname = values[3];
                            obj.address1 = values[4];
                            obj.apt = values[5];
                            obj.city = values[6];
                            obj.state = values[7];
                            obj.zipcode = values[8];
                            obj.phone = values[9];
                            obj.email = values[18];
                            obj.cardholder = values[19];
                            obj.cardnumber = values[20];
                            obj.expiry = values[21];
                            obj.cvv = values[22];
                                obj.Billingregion = values[23];
                            ls.Add(obj);
                            }
                            else
                            {
                                if(values[0] == pr)
                                {
                                    count++;
                                    obj.name = values[0];
                                    obj.Country = values[1];
                                    obj.firstname = values[2];
                                    obj.lastname = values[3];
                                    obj.address1 = values[4];
                                    obj.apt = values[5];
                                    obj.city = values[6];
                                    obj.state = values[7];
                                    obj.zipcode = values[8];
                                    obj.phone = values[9];
                                    obj.email = values[18];
                                    obj.cardholder = values[19];
                                    obj.cardnumber = values[20];
                                    obj.expiry = values[21];
                                    obj.cvv = values[22];
                                    obj.Billingregion = values[23];
                                    ls.Add(obj);
                                }
                            }
                        }
                    }
                    reader.Dispose();
                }
            }
            return ls;

        }

        private void Pc_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                Grid u = sender as Grid;
                TextBlock t = u.FindName("pname") as TextBlock;
                TextBlock t1 = u.FindName("countryf") as TextBlock;
                //MessageBox.Show(t.Text);
                this.NavigationService.Navigate(new AdditionalProfileInfo(t.Text, t1.Text, true));
            }
        }
       

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.DefaultExt = ".csv"; // Required file extension 
            fileDialog.Filter = "csv files (*.csv)|*.csv|txt files (*.txt)|*.txt|All files (*.*)|*.*"; // Optional file extensions

            //fileDialog.ShowDialog();

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamReader sr = new
                System.IO.StreamReader(fileDialog.FileName);
                String line;
                var count = 0;
                var filepath = "Data/ProfileData/profiles.csv";
                if (!System.IO.Directory.Exists("Data/ProfileData"))
                    System.IO.Directory.CreateDirectory("Data/ProfileData");//.Dispose();
                if (!System.IO.File.Exists(filepath))
                    System.IO.File.Create(filepath).Dispose();
                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    //before your loop
                    var csv = new StringBuilder();

                    //in your loop
                    var newLine = parts;
                    csv.AppendLine(line);
                    //after your loop

                    File.AppendAllText(filepath, csv.ToString());
                    count++;
                }
                this.NavigationService.Navigate(new ProfileSection());

            }
        }
        private void btnaddProfile_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new CreateProfile(""));
            //profile.Visibility = Visibility.Hidden;
            //profileinfo.Visibility = Visibility.Hidden;

            //cp.createProfile.Visibility = Visibility.Visible;
            //cp.createprofileinfo.Visibility = Visibility.Visible;
            //proxiestester.Visibility = Visibility.Hidden;
            //proxiestesterinfo.Visibility = Visibility.Hidden;
        }

        private void Export_Profiles_Click(object sender, RoutedEventArgs e)
        {
            var filepath = "Data/ProfileData/profiles.csv";
            if (!System.IO.Directory.Exists("Data/ProfileData"))
                System.IO.Directory.CreateDirectory("Data/ProfileData");//.Dispose();
            if (!System.IO.File.Exists(filepath))
                System.IO.File.Create(filepath).Dispose();
            SaveFileDialog savefile = new SaveFileDialog();
            // set a default file name
            savefile.FileName = ".csv";
            // set filters - this can be done in properties as well
            savefile.Filter = "csv files (*.csv)|*.csv|txt files (*.txt)|*.txt|All files (*.*)|*.*";

            if (savefile.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter sw = new StreamWriter(savefile.FileName))

                using (StreamReader sr = new StreamReader(filepath))
                {
                    String line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        
                            string text = line;
                            sw.WriteLine(text);
                    }
                    sw.Dispose();
                    sr.Dispose();
                }
            }
        }
    }
}

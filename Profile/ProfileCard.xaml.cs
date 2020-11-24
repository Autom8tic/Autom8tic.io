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

namespace Dashboard1.Profile
{
    /// <summary>
    /// Interaction logic for UserControl2.xaml
    /// </summary>
    public partial class ProfileCard : UserControl
    {
        public ProfileCard(string Name, string Country, string datetime = "")
        {
            InitializeComponent();
            pname.Text = Name;
            TCalendar.Text = datetime;
            countryf.Text = Country;
            countryf.ToolTip = Country;
            BtnRemoveProfile.Cursor = Cursors.Hand;
            sample_profile.Cursor = Cursors.Hand;
           
        }

        private void BtnRemoveProfile_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            var filepath = "Data/ProfileData/profiles.csv";
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
                        if (parts[0].Trim() == pname.Text.Trim())
                        {
                            sr.Dispose();
                              string text = File.ReadAllText(filepath);
                            text = text.Replace(line, "").Remove(count,1);
                            File.WriteAllText(filepath, text);
                            this.Visibility = Visibility.Hidden;
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
}

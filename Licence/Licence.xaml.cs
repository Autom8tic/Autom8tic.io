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
using System.Windows.Shapes;

namespace Dashboard1.Licence
{
    /// <summary>
    /// Interaction logic for Licence.xaml
    /// </summary>
    public partial class Licence : Window
    {
        public Licence()
        {
            InitializeComponent();
        }

        private void btn_Validate_Click(object sender, RoutedEventArgs e)
        {
            try {
                if (key.Text.Trim() != "")
                {
                    var filepath = "Data/ProfileData/licence.csv";
                    if (!System.IO.File.Exists(filepath))
                        saveKey();
                    else
                        updateKey();

                    var result = ValidateLicence.validateLicnece();
                    if (result.Contains("Valid:1"))
                    {
                        var days = result.Split('|')[1].Replace("Days:", "");
                        MessageBox.Show("The Product is activated for " + days.ToString() + " days.");
                        this.Close();
                        MainWindow main = new MainWindow();
                        main.Activate();
                        main.Show();
                    }
                    else
                    {
                        result = result.Replace("Valid:0|", "");
                        MessageBox.Show(result);
                    }
                }
                else
                {
                    key.BorderBrush = new SolidColorBrush(Colors.Red);
                    key.BorderThickness = new Thickness(1);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void saveKey()
        {
            var filepath = "Data/ProfileData/licence.csv";
            if(!System.IO.Directory.Exists("Data/ProfileData"))
                System.IO.Directory.CreateDirectory("Data/ProfileData");//.Dispose();
            if (!System.IO.File.Exists(filepath))
                System.IO.File.Create(filepath).Close();//.Dispose();

            using (StreamReader sr = new StreamReader(filepath))
            {
                CreateProfile p = new CreateProfile(null);
                if (!p.CheckExisting("licence.csv", key.Text))
                {
                    sr.Dispose();
                    var csv = new StringBuilder();

                    //in your loop
                    var newLine = string.Format("{0},{1}",
                        key.Text, DateTime.Now.ToString()
                        );
                    csv.AppendLine(newLine);
                    //after your loop
                    File.AppendAllText(filepath, csv.ToString());

                }
            }
        }

        public void updateKey()
        {
            var filepath = "Data/ProfileData/licence.csv";
            using (StreamReader sr = new StreamReader(filepath))
            {
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    
                        sr.Dispose();
                        var newData = key.Text + "," + DateTime.Now.ToString();
                        string text = File.ReadAllText(filepath);
                        text = text.Replace(line, newData);
                        File.WriteAllText(filepath, text);
                        break;
                }
            }
        }

    }
}

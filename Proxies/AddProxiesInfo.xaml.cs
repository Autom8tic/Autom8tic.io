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

namespace Dashboard1.Proxies
{
    /// <summary>
    /// Interaction logic for AddProxiesInfo.xaml
    /// </summary>
    public partial class AddProxiesInfo : Page
    {
        public AddProxiesInfo()
        {
            InitializeComponent(); populateList();
        }

        public void populateList()
        {
            List<ProxyList> pr = GetProxyLists();
            List<MainWindow.ComboData> ListData = new List<MainWindow.ComboData>();
            ListData.Add(new MainWindow.ComboData { Id = "Select Proxy", Value = "" });

            foreach (ProxyList p in pr)
            {
                MainWindow.ComboData c = new MainWindow.ComboData();
                c.Id = p.name;
                c.Value = p.name;
                ListData.Add(c);
            }
            proxy_list.ItemsSource = ListData;
            proxy_list.DisplayMemberPath = "Id";
            proxy_list.SelectedValuePath = "Value";
            proxy_list.SelectedValue = "";
        }

        public class ProxyList
        {
            public string name { get; set; }
            public string values { get; set; }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //AddProxiesTest protes = new AddProxiesTest();
            this.NavigationService.Navigate(new AddProxiesTest());
            //proxies1.Visibility = Visibility.Hidden;
            //proxiesinfo.Visibility = Visibility.Hidden;
            //protes.proxiestester.Visibility = Visibility.Visible;
            //protes.proxiestesterinfo.Visibility = Visibility.Visible;
        }

        private void btn_saveProxies_Click(object sender, RoutedEventArgs e)
        {
            CreateProfile cp = new CreateProfile("");

            if (proxyName.Text != "")
            {
                List<MainWindow.ComboData> ListData = new List<MainWindow.ComboData>();
                ListData.Add(new MainWindow.ComboData { Id = proxyName.Text, Value = proxyName.Text });
                //proxy_list.ItemsSource = ListData;
                //proxy_list.DisplayMemberPath = "Value";
                //proxy_list.SelectedValuePath = "Id";
                //proxy_list.SelectedValue = proxyName.Text;
                saveProxies();
                MessageBox.Show("Proxy Created With Name: " + proxyName.Text);
            }
            else if(proxy_list.SelectedIndex != 0)
            {
                editProxies();
                MessageBox.Show("Proxy Saved!!");
            }
            else
            {
                cp.profile_name.BorderBrush = new SolidColorBrush(Colors.Red);
            }
        }


        public void getData(ComboBox c)
        {
            var filepath = "Data/ProfileData/proxies.csv";
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
                            if(c.SelectedValue != null) { 
                            if (values[0] == c.SelectedValue.ToString())
                            {
                                try
                                {
                                    proxyValues.Document.Blocks.Clear();
                                    proxyValues.Document.Blocks.Add(new Paragraph(new Run(values[1].Replace("|", "\r\n"))));
                                }
                                catch
                                {

                                }
                                }
                                else if (c.SelectedValue.ToString() == "")
                                {
                                    proxyValues.Document.Blocks.Clear();
                                }
                            }
                            else 
                            {
                                proxyValues.Document.Blocks.Clear();
                            }
                        }
                    }
                    reader.Dispose();
                }
                if (count > 0)
                {


                }
            }
            else
            {
            }
        }

        public static List<ProxyList> GetProxyLists(string pr = "") {
            var filepath = "Data/ProfileData/proxies.csv";
            List<ProxyList> ls = new List<ProxyList>();
            if (File.Exists(filepath))
            {
                var count = 0;
                using (var reader = new StreamReader(filepath))
                {
                    List<string> listA = new List<string>();
                    List<string> listB = new List<string>();
                    while (!reader.EndOfStream)
                    {
                        ProxyList obj = new ProxyList();
                        var line = reader.ReadLine();
                        var values = line.Split(',');
                        if (line == "")
                        {

                        }
                        else
                        {
                            if (pr == "")
                            {
                                count++;
                                obj.name = values[0];
                                obj.values = values[1];
                                ls.Add(obj);
                            }
                            else
                            {
                                if (values[0] == pr)
                                {
                                    count++;
                                    obj.name = values[0];
                                    obj.values = values[1];
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

        public bool saveProxies()
        {
            var filepath = "Data/ProfileData/proxies.csv";
            if (!System.IO.Directory.Exists("Data/ProfileData"))
                System.IO.Directory.CreateDirectory("Data/ProfileData");//.Dispose();
            if (!System.IO.File.Exists(filepath))
                System.IO.File.Create(filepath).Close();//.Dispose();
            bool result = false;

            using (StreamReader sr = new StreamReader(filepath))
            {
                CreateProfile p = new CreateProfile(null);
                if (!p.CheckExisting("proxies.csv", proxyName.Text))
                {
                    sr.Dispose();
                    var csv = new StringBuilder();
                    string richText = new TextRange(proxyValues.Document.ContentStart, proxyValues.Document.ContentEnd).Text.Replace("\r\n","|").Replace(",","|");

                    //in your loop
                    var newLine = string.Format("{0},{1},{2}",
                        proxyName.Text, richText, DateTime.Now.ToString()
                        );
                    csv.AppendLine(newLine);
                    //after your loop
                    File.AppendAllText(filepath, csv.ToString());
                    populateList();
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

        public bool editProxies()
        {
            var filepath = "Data/ProfileData/proxies.csv";
            if (!System.IO.Directory.Exists("Data/ProfileData"))
                System.IO.Directory.CreateDirectory("Data/ProfileData");//.Dispose();
            if (!System.IO.File.Exists(filepath))
                System.IO.File.Create(filepath).Close();//.Dispose();
            using (StreamReader sr = new StreamReader(filepath))
            {
                String line;
                string richText = new TextRange(proxyValues.Document.ContentStart, proxyValues.Document.ContentEnd).Text.Replace("\r\n", "|").Replace(",", "|");

                while ((line = sr.ReadLine()) != null)
                {
                    string[] parts = line.Split(',');
                    if (parts[0] == proxy_list.Text.ToString())
                    {
                        sr.Dispose();
                        var newData = parts[0] + "," + richText + "," + line[2];
                        string text = File.ReadAllText(filepath);
                        text = text.Replace(line, newData);
                        File.WriteAllText(filepath, text);
                        populateList();
                        break;
                    }

                }
            }

            return true;
        }

        private void proxy_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox c = sender as ComboBox;
            getData(c);
        }

        private void BtnRemoveProfile_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            var filepath = "Data/ProfileData/proxies.csv";
            if (File.Exists(filepath))
            {
                using (StreamReader sr = new StreamReader(filepath))
                {
                    String line;
                    var count = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (proxy_list.SelectedValue != null)
                        {
                            if (parts[0].Trim() == proxy_list.Text.ToString().Trim())
                            {
                                sr.Dispose();
                                string text = File.ReadAllText(filepath);
                                text = text.Replace(line, "").Remove(count, 1);
                                File.WriteAllText(filepath, text);
                                populateList();
                                break;
                            }

                        }
                        count++;
                    }
                }
            }
            else
            {

            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using static Dashboard1.Proxies.AddProxiesInfo;

namespace Dashboard1.Proxies
{
    /// <summary>
    /// Interaction logic for AddProxiesTest.xaml
    /// </summary>
    public partial class AddProxiesTest : Page
    {
        public AddProxiesTest()
        {
            InitializeComponent(); populateList();
        }

        public void populateList()
        {
            //AddProxiesInfo info = new AddProxiesInfo();
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

        public string getData(ComboBox c)
        {
            string result = "";
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
                            if (c.SelectedValue != null)
                            {
                                if (values[0] == c.SelectedValue.ToString())
                                {
                                    try
                                    {
                                        result = values[1];
                                    }
                                    catch
                                    {

                                    }
                                }
                                else if (c.SelectedValue.ToString() == "")
                                {
                                }
                            }
                            else
                            {
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
            return result;
        }

        private void btn_backTestProxy_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AddProxiesInfo());
        }

        private void btn_proxyTesterStore(object sender, RoutedEventArgs e)
        {
            Popup_ProxyTester.IsOpen = true;
        }
        private void btn_proxyTesterStoreClose(object sender, RoutedEventArgs e)
        {
            Popup_ProxyTester.IsOpen = false;
        }
        private void Navigate_Click(object sender, RoutedEventArgs e)//By Prince Jain 
        {
            Captcha c = new Captcha();
            c.Show();
            c.Activate();
        }

        private void test_proxy_Click(object sender, RoutedEventArgs e)
        {
                var proxies = getData(proxy_list);
                var pr = proxies.Split('|');
                 this.NavigationService.Navigate(new ProxyTestResults(pr,site_url.Text));

        }

        private void proxy_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}

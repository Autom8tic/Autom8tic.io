using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace Dashboard1.Proxies
{
    /// <summary>
    /// Interaction logic for ProxyTestResults.xaml
    /// </summary>
    public partial class ProxyTestResults : Page
    {
        public ProxyTestResults(string[] pr, string url)
        {
            InitializeComponent();
            populateList(pr, url);
        }

        public void populateList(string[] pr, string url)
        {
            var count = 0;
            list_view.Children.Clear();
            foreach (var p in pr)
            {
                if (p != null && p != "")
                {
                    count++;
                    try
                    {
                        //WebClient client = new WebClient();
                        //WebProxy proxy1 = new WebProxy(p.Split(':')[0] + ":" + p.Split(':')[1], true);
                        //proxy1.Credentials = new NetworkCredential(p.Split(':')[2], p.Split(':')[3]);
                        ////proxy1.UseDefaultCredentials = true;
                        //WebRequest.DefaultWebProxy = proxy1;
                        //client.Proxy = proxy1;
                        //if (client.IsBusy) {
                        //    System.Threading.Thread.Sleep(2000);
                        //}
                        //else{
                        //    var s = client.Headers;
                        //    string str = client.DownloadString(url);
                        //}

                        BackgroundWorker bw = new BackgroundWorker();
                        bw.DoWork += Bw_StartTest;
                        bw.RunWorkerCompleted += Bw_StartTestCompleted;
                        bw.WorkerSupportsCancellation = true;
                        //Parameter you need to work in Background-Thread for example your strings
                        string[] param = new[] { p, url, count.ToString() };

                        //Start work
                        bw.RunWorkerAsync(param);

                    }
                    catch (Exception ex)
                    {
                        ProxyTestList lst = new ProxyTestList(count, p, url, ex.Message);
                        list_view.Children.Clear();
                        list_view.Children.Add(lst);
                    }
                }
            }
        }


        private void Bw_StartTest(object sender, DoWorkEventArgs e)
        {
            string[] param = e.Argument as string[];
            var p = param[0];
            var url = param[1];
            var count =param[2];
            try {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebProxy myproxy = new WebProxy(p.Split(':')[0] + ":" + p.Split(':')[1], true);
            if(p.Split(':').Length > 2)
                {
                    var username = p.Split(':')[2];
                    var password = p.Split(':')[3];
                    myproxy.Credentials = new NetworkCredential(username, password);
            }
            //proxy1.UseDefaultCredentials = true;
            WebRequest.DefaultWebProxy = myproxy;
            myproxy.BypassProxyOnLocal = false;
            request.Proxy = myproxy;
            request.Method = "GET";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string[] param1 = { count, p, url, response.StatusCode + ":" + response.StatusDescription };
                e.Result = param1;
            }
            catch(Exception ex)
            {
                string[] param1 = { count, p, url, "Bad Proxy:" + ex.Message};
                e.Result = param1;
            }
        }
        private void Bw_StartTestCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            string[] param1 = e.Result as string[];
            ProxyTestList lst = new ProxyTestList(Convert.ToInt32(param1[0]), param1[1].ToString(), param1[2].ToString(), param1[3].ToString());
            laoder_space.Children.Clear();
            //list_view.Children.Clear();
            list_view.Children.Add(lst);
        }


        private void btn_backTestProxy_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AddProxiesTest());
        }
    }
}

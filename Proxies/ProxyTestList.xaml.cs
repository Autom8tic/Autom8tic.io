using System;
using System.Collections.Generic;
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
    /// Interaction logic for ProxyTestList.xaml
    /// </summary>
    public partial class ProxyTestList : UserControl
    {
        public ProxyTestList(int pcount = 0, string pproxy = "", string purl ="", string pstatus = "")
        {
            InitializeComponent();
            txt_srNo.Text =  pcount.ToString();
            txt_proxy.Text =  pproxy;
            txt_url.Text =  purl;
            txt_status.Text = pstatus;
            txt_status.ToolTip = pstatus;
        }
    }
}

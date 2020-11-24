using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Dashboard1.Dashboard
{
    /// <summary>
    /// Interaction logic for Dashboardndex.xaml
    /// </summary>
    public partial class Dashboardndex : Page
    {
        public Dashboardndex()
        {
            InitializeComponent();
        }

        private void twitter_btn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://twitter.com/");
        }
        private void home_btn_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://swxpe.io/");
        }
        private void checkforupdate_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://swxpe.io/");
        }

        private void OnNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(e.Uri.AbsoluteUri);
            e.Handled = true;
        }
    }
}

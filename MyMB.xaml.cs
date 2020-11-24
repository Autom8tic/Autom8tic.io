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
using System.Windows.Shapes;

namespace Dashboard1
{
    /// <summary>
    /// Interaction logic for MyMB.xaml
    /// </summary>
    public partial class MyMB : Window
    {
        public string ReturnString { get; set; }
        public MyMB(string content, Visibility ok, Visibility cancel, Visibility close, string part)
        {
            InitializeComponent();
            btnOk.Visibility = ok;
            btnCancel.Visibility = cancel;
            btnClose.Visibility = close;
            label.Content = content;
            partLabel.Content = part;
            WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
        }

        private void gBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { };
        }

        
        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ReturnString = "-1";
            Close();
        }
        
        private void btnReturnValue_Click(object sender, RoutedEventArgs e)
        {
            ReturnString = ((Button)sender).Uid.ToString();
            
            if (ReturnString == "1")
            {
                if(partLabel.Content == "ProfileSection")
                {
                    CreateProfile cp = new CreateProfile("profile");
                }
            }
            this.Close();
        }
    }
}

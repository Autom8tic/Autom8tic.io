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

namespace Dashboard1.Captchas
{
    /// <summary>
    /// Interaction logic for CreateProfileCaptcha.xaml
    /// </summary>
    public partial class CreateProfileCaptcha : Page
    {
        public CreateProfileCaptcha()
        {
            InitializeComponent();
        }
        private void btn_openSolver_Cancel(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBoxResult = System.Windows.MessageBox.Show("Are you sure?", "Cancel Action", System.Windows.MessageBoxButton.YesNo);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                //openSolver.Visibility = Visibility.Hidden;
                this.NavigationService.Navigate(new ShowCaptchaProfile());
            }

        }
        private void btn_createOpenSolver(object sender, RoutedEventArgs e)
        {
            //openSolver.Visibility = Visibility.Hidden;
            openSolverInfo.Visibility = Visibility.Hidden;

        }
    }
}

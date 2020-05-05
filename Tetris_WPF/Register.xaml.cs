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

namespace WpfApp1
{
    /// <summary>
    /// Logique d'interaction pour Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        public Register()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            Tuple<bool, string> response = SingleHttpClientInstance.Register(email.Text, password.Password, passwordConfirm.Password);
            if(response.Item1)
            {
                error_msg.Visibility = Visibility.Visible;
                error_msg.Text = response.Item2;
            }
            else
            {
                error_msg.Visibility = Visibility.Hidden;
                new login().Show();
                Close();
            }
        }

        private void back_Click(object sender, RoutedEventArgs e)
        {
            error_msg.Visibility = Visibility.Hidden;
            new login().Show();
            Close();
        }
    }
}

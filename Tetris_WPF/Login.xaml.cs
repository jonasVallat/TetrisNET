using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    /// Logique d'interaction pour login.xaml
    /// </summary>
    public partial class login : Window
    {

        public login()
        {
            InitializeComponent();
        }

        private void Login_btn_Click(object sender, RoutedEventArgs e)
        {
            Tuple<bool, string> result = SingleHttpClientInstance.Login(email.Text, password.Password);
            if(result.Item1)
            {
                error_msg.Visibility = Visibility.Visible;
                error_msg.Text = result.Item2;
            }
            else
            {
                error_msg.Visibility = Visibility.Hidden;
                Application.Current.Properties["token"] = result.Item2;

                

                new Tetris.MainWindow().Show();
                Close();
            }
        }

        private void Register_btn_Click(object sender, RoutedEventArgs e)
        {
            
            new Register().Show();
            Close();
        }
    }
}

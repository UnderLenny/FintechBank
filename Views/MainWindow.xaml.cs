using FintechBank;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FintechBank
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (Authenticate(username, password))
            {
                MessageBox.Show("Login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                // Открой главное окно или личный кабинет
                var userDashboard = new UserDashboardWindow();
                userDashboard.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RegisterText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var registerWindow = new RegisterWindow();
            registerWindow.Show();
            this.Close();
        }

        private bool Authenticate(string username, string password)
        {
            // Реализуй логику аутентификации
            // Временно возвращаем true для примера
            return true;
        }
    }
}

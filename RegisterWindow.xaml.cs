using FintechBank;
using System.Windows;
using System.Windows.Input;

namespace FintechBank
{
    public partial class RegisterWindow : Window
    {
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = RegUsernameTextBox.Text;
            string password = RegPasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (password == confirmPassword)
            {
                Register(username, password);
                MessageBox.Show("Registration successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                var loginWindow = new MainWindow();
                loginWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Passwords do not match.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Register(string username, string password)
        {
            // Реализуй логику регистрации
            // Временно ничего не делаем для примера
        }

        private void LoginText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}

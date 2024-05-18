using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using FintechBank.Models;

namespace FintechBank
{
    public partial class LoginWindow : Window
    {
        private readonly Connection connection = new Connection();

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;

            if (Login(email, HashPassword(password)))
            {
                MessageBox.Show("Login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                // Здесь можно открыть основное окно приложения
                var mainAppWindow = new UserDashboardWindow();
                mainAppWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Login failed. Invalid email or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool Login(string email, string passwordHash)
        {
            try
            {
                connection.openConnection();

                string query = "SELECT COUNT(*) FROM Users WHERE Email = @Email AND PasswordHash = @PasswordHash;";
                SqlCommand command = new SqlCommand(query, connection.getConnection());
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                int count = (int)command.ExecuteScalar();

                return count > 0;
            }
            catch (Exception ex)
            {
                // Логирование ошибок
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            finally
            {
                connection.closeConnection();
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void RegisterText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var registerWindow = new RegisterWindow();
            registerWindow.Show();
            this.Close();
        }
    }
}

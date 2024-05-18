using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Input;
using FintechBank.Models;

namespace FintechBank
{
    public partial class RegisterWindow : Window
    {
        private readonly Connection connection = new Connection();

        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (password == confirmPassword)
            {
                string passwordHash = HashPassword(password);
                if (Register(firstName, lastName, email, passwordHash))
                {
                    MessageBox.Show("Registration successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    var loginWindow = new MainWindow();
                    loginWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Registration failed. Email might already be in use.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Passwords do not match.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private bool Register(string firstName, string lastName, string email, string passwordHash)
        {
            try
            {
                connection.openConnection();

                // Проверка, существует ли уже пользователь с таким email
                string checkQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email;";
                SqlCommand checkCommand = new SqlCommand(checkQuery, connection.getConnection());
                checkCommand.Parameters.AddWithValue("@Email", email);
                int count = (int)checkCommand.ExecuteScalar();

                if (count > 0)
                {
                    return false; // Email уже используется
                }

                // Вставка нового пользователя
                string query = "INSERT INTO Users (FirstName, LastName, Email, PasswordHash) VALUES (@FirstName, @LastName, @Email, @PasswordHash);";
                SqlCommand command = new SqlCommand(query, connection.getConnection());
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                int result = command.ExecuteNonQuery();

                return result > 0;
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

        private void LoginText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}

using FintechBank.Models;
using FintechBank.Views;
using System;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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

            int currentUserId;
            string userRole;

            if (Login(email, HashPassword(password), out currentUserId, out userRole))
            {
                MessageBox.Show("Login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                try
                {
                    if (userRole == "Admin")
                    {
                        var adminDashboardWindow = new AdminDashboardWindow(currentUserId);
                        adminDashboardWindow.Show();
                    }
                    else if (userRole == "User")
                    {
                        var userDashboardWindow = new UserDashboardWindow(currentUserId);
                        userDashboardWindow.Show();
                    }
                    else
                    {
                        MessageBox.Show("Unknown user role.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to open the dashboard window: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                this.Close();
            }
            else
            {
                MessageBox.Show("Login failed. Invalid email or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool Login(string email, string passwordHash, out int currentUserId, out string userRole)
        {
            currentUserId = -1;
            userRole = "";

            try
            {
                connection.openConnection();

                string query = @"SELECT u.UserID, r.RoleName
                                 FROM Users u
                                 JOIN UserRoles ur ON u.UserID = ur.UserID
                                 JOIN Roles r ON ur.RoleID = r.RoleID
                                 WHERE u.Email = @Email AND u.PasswordHash = @PasswordHash;";
                SqlCommand command = new SqlCommand(query, connection.getConnection());
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    currentUserId = reader.GetInt32(0);
                    userRole = reader.GetString(1);
                    return true;
                }
                else
                {
                    MessageBox.Show("No matching user found.", "Debug", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return false;
                }
            }
            catch (Exception ex)
            {
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

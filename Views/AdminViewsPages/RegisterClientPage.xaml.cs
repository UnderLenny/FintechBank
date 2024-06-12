using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using FintechBank.Models; // Пространство имен с вашими моделями
using System.Data.SqlClient;

namespace FintechBank.Views.AdminViewsPages
{
    /// <summary>
    /// Логика взаимодействия для RegisterClientPage.xaml
    /// </summary>
    public partial class RegisterClientPage : Page
    {
        private readonly Connection _connection;

        public RegisterClientPage()
        {
            InitializeComponent();
            _connection = new Connection();
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
                int userID = Register(firstName, lastName, email, passwordHash);
                if (userID != -1)
                {
                    MessageBox.Show("Регистрация прошла успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    int accountID = CreateAccount(userID);
                    CreateCard(accountID);
                }
                else
                {
                    MessageBox.Show("Регистрация не удалась. Возможно, email уже используется.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Пароли не совпадают.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private string GenerateAccountNumber()
        {
            Random random = new Random();
            string accountNumber = "";
            for (int i = 0; i < 10; i++)
            {
                int digit = random.Next(0, 10);
                accountNumber += digit.ToString();
            }
            return accountNumber;
        }

        private string GenerateCardNumber()
        {
            Random random = new Random();
            StringBuilder cardNumber = new StringBuilder();
            for (int i = 0; i < 16; i++)
            {
                int digit = random.Next(0, 10);
                cardNumber.Append(digit);
            }
            return cardNumber.ToString();
        }

        private int Register(string firstName, string lastName, string email, string passwordHash)
        {
            try
            {
                _connection.openConnection();
                SqlConnection conn = _connection.getConnection();

                string checkEmailQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                using (SqlCommand cmd = new SqlCommand(checkEmailQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    int count = (int)cmd.ExecuteScalar();
                    if (count > 0)
                    {
                        return -1;
                    }
                }
                string insertUserQuery = "INSERT INTO Users (FirstName, LastName, Email, PasswordHash) OUTPUT INSERTED.UserID VALUES (@FirstName, @LastName, @Email, @PasswordHash)";
                int userID;
                using (SqlCommand cmd = new SqlCommand(insertUserQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@FirstName", firstName);
                    cmd.Parameters.AddWithValue("@LastName", lastName);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    userID = (int)cmd.ExecuteScalar();
                }

                AssignRoleToUser(userID, "User", conn);

                return userID;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
            finally
            {
                _connection.closeConnection();
            }
        }

        private void AssignRoleToUser(int userID, string roleName, SqlConnection conn)
        {
            string getRoleIDQuery = "SELECT RoleID FROM Roles WHERE RoleName = @RoleName";
            int roleID;
            using (SqlCommand cmd = new SqlCommand(getRoleIDQuery, conn))
            {
                cmd.Parameters.AddWithValue("@RoleName", roleName);
                roleID = (int)cmd.ExecuteScalar();
            }

            string insertUserRoleQuery = "INSERT INTO UserRoles (UserID, RoleID) VALUES (@UserID, @RoleID)";
            using (SqlCommand cmd = new SqlCommand(insertUserRoleQuery, conn))
            {
                cmd.Parameters.AddWithValue("@UserID", userID);
                cmd.Parameters.AddWithValue("@RoleID", roleID);
                cmd.ExecuteNonQuery();
            }
        }

        private int CreateAccount(int userID)
        {
            try
            {
                _connection.openConnection();
                SqlConnection conn = _connection.getConnection();

                decimal initialBalance = 1000;
                string accountNumber = GenerateAccountNumber();

                string insertAccountQuery = "INSERT INTO Accounts (UserID, AccountNumber, Balance) OUTPUT INSERTED.AccountID VALUES (@UserID, @AccountNumber, @Balance)";
                int accountID;
                using (SqlCommand cmd = new SqlCommand(insertAccountQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userID);
                    cmd.Parameters.AddWithValue("@AccountNumber", accountNumber);
                    cmd.Parameters.AddWithValue("@Balance", initialBalance);
                    accountID = (int)cmd.ExecuteScalar();
                }

                return accountID;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
            finally
            {
                _connection.closeConnection();
            }
        }

        private void CreateCard(int accountID)
        {
            try
            {
                _connection.openConnection();
                SqlConnection conn = _connection.getConnection();

                string cardNumber = GenerateCardNumber();
                string cardType = "Debit";
                DateTime expirationDate = DateTime.Now.AddYears(3);
                int cardStatusID = 1;

                string insertCardQuery = "INSERT INTO Cards (AccountID, CardNumber, CardType, ExpirationDate, CardStatusID) VALUES (@AccountID, @CardNumber, @CardType, @ExpirationDate, @CardStatusID)";
                using (SqlCommand cmd = new SqlCommand(insertCardQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@AccountID", accountID);
                    cmd.Parameters.AddWithValue("@CardNumber", cardNumber);
                    cmd.Parameters.AddWithValue("@CardType", cardType);
                    cmd.Parameters.AddWithValue("@ExpirationDate", expirationDate);
                    cmd.Parameters.AddWithValue("@CardStatusID", cardStatusID);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                _connection.closeConnection();
            }
        }
    }
}

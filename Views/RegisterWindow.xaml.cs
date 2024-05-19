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
                int userID = Register(firstName, lastName, email, passwordHash);
                if (userID != -1)
                {
                    MessageBox.Show("Registration successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    int accountID = CreateAccount(userID); 
                    CreateCard(accountID); 
                    AssignRole(userID);
                    var loginWindow = new LoginWindow();
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
                connection.openConnection();

                // Проверка, существует ли уже пользователь с таким email
                string checkQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email;";
                SqlCommand checkCommand = new SqlCommand(checkQuery, connection.getConnection());
                checkCommand.Parameters.AddWithValue("@Email", email);
                int count = (int)checkCommand.ExecuteScalar();

                if (count > 0)
                {
                    return -1; 
                }

                string query = "INSERT INTO Users (FirstName, LastName, Email, PasswordHash) OUTPUT INSERTED.UserID VALUES (@FirstName, @LastName, @Email, @PasswordHash);";
                SqlCommand command = new SqlCommand(query, connection.getConnection());
                command.Parameters.AddWithValue("@FirstName", firstName);
                command.Parameters.AddWithValue("@LastName", lastName);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@PasswordHash", passwordHash);
                int userID = (int)command.ExecuteScalar();

                return userID;
            }
            catch (Exception ex)
            {
                // Логирование ошибок
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
            finally
            {
                connection.closeConnection();
            }
        }

        private int CreateAccount(int userID)
        {
            try
            {
                connection.openConnection();

                decimal initialBalance = 1000;
                string accountNumber = GenerateAccountNumber(); 

                string accountQuery = "INSERT INTO Accounts (UserID, AccountNumber, Balance) OUTPUT INSERTED.AccountID VALUES (@UserID, @AccountNumber, @Balance);";
                SqlCommand accountCommand = new SqlCommand(accountQuery, connection.getConnection());
                accountCommand.Parameters.AddWithValue("@UserID", userID); 
                accountCommand.Parameters.AddWithValue("@AccountNumber", accountNumber);
                accountCommand.Parameters.AddWithValue("@Balance", initialBalance);
                int accountID = (int)accountCommand.ExecuteScalar();

                return accountID;
            }
            catch (Exception ex)
            {
                // Логирование ошибок
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }
            finally
            {
                connection.closeConnection();
            }
        }


        private void CreateCard(int accountID) 
        {
            try
            {
                connection.openConnection();

                string cardNumber = GenerateCardNumber(); 
                string cardType = "Debit";
                DateTime expirationDate = DateTime.Now.AddYears(3); 
                int cardStatusID = 1; 

                string cardQuery = "INSERT INTO Cards (AccountID, CardNumber, CardType, ExpirationDate, CardStatusID) VALUES (@AccountID, @CardNumber, @CardType, @ExpirationDate, @CardStatusID);";
                SqlCommand cardCommand = new SqlCommand(cardQuery, connection.getConnection());
                cardCommand.Parameters.AddWithValue("@AccountID", accountID); 
                cardCommand.Parameters.AddWithValue("@CardNumber", cardNumber);
                cardCommand.Parameters.AddWithValue("@CardType", cardType);
                cardCommand.Parameters.AddWithValue("@ExpirationDate", expirationDate);
                cardCommand.Parameters.AddWithValue("@CardStatusID", cardStatusID);
                cardCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // Логирование ошибок
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                connection.closeConnection();
            }
        }

        private void AssignRole(int userID)
        {
            try
            {
                connection.openConnection();

                int roleID = 2; 

                string userRoleQuery = "INSERT INTO UserRoles (UserID, RoleID) VALUES (@UserID, @RoleID);";
                SqlCommand userRoleCommand = new SqlCommand(userRoleQuery, connection.getConnection());
                userRoleCommand.Parameters.AddWithValue("@UserID", userID);
                userRoleCommand.Parameters.AddWithValue("@RoleID", roleID);
                userRoleCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // Логирование ошибок
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                connection.closeConnection();
            }
        }

        private void LoginText_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var loginWindow = new LoginWindow();
            loginWindow.Show();
            this.Close();
        }
    }
}

using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using FintechBank.Models;
using System.Data;

namespace FintechBank.Views.AdminViewsPages
{
    /// <summary>
    /// Логика взаимодействия для ExportTransactionsPage.xaml
    /// </summary>
    public partial class ExportTransactionsPage : Page
    {
        public ExportTransactionsPage()
        {
            InitializeComponent();
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Создаем объект подключения
                Connection dbConnection = new Connection();
                dbConnection.openConnection();
                SqlConnection connection = dbConnection.getConnection();

                // Запрос для получения транзакций пользователей
                string query = @"
                    SELECT 
                        U.UserID,
                        U.FirstName,
                        U.LastName,
                        T.TransactionID,
                        T.Amount,
                        TT.TransactionTypeName,
                        T.Description,
                        T.CreatedAt
                    FROM Transactions T
                    JOIN Accounts A ON T.SenderAccountID = A.AccountID OR T.ReceiverAccountID = A.AccountID
                    JOIN Users U ON A.UserID = U.UserID
                    JOIN TransactionTypes TT ON T.TransactionTypeID = TT.TransactionTypeID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);

                    // Создание файла Excel
                    using (var workbook = new XLWorkbook())
                    {
                        var worksheet = workbook.Worksheets.Add("User Transactions");
                        worksheet.Cell(1, 1).Value = "UserID";
                        worksheet.Cell(1, 2).Value = "FirstName";
                        worksheet.Cell(1, 3).Value = "LastName";
                        worksheet.Cell(1, 4).Value = "TransactionID";
                        worksheet.Cell(1, 5).Value = "Amount";
                        worksheet.Cell(1, 6).Value = "TransactionType";
                        worksheet.Cell(1, 7).Value = "Description";
                        worksheet.Cell(1, 8).Value = "CreatedAt";

                        for (int i = 0; i < dataTable.Rows.Count; i++)
                        {
                            for (int j = 0; j < dataTable.Columns.Count; j++)
                            {
                                var cellValue = dataTable.Rows[i][j];
                                if (cellValue != DBNull.Value)
                                {
                                    worksheet.Cell(i + 2, j + 1).Value = cellValue.ToString();
                                }
                                else
                                {
                                    worksheet.Cell(i + 2, j + 1).Value = string.Empty;
                                }
                            }
                        }

                        string filePath = "D:\\UserTransactions.xlsx";
                        workbook.SaveAs(filePath);
                        MessageBox.Show("Транзакции экспортированы успешно в " + filePath);
                    }
                }

                dbConnection.closeConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при экспорте транзакций: " + ex.Message);
            }
        }
    }
}
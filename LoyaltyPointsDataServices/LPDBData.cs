using LoyaltyPointsModels;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyPointsDataServices
{
        public class LPDBData : ILPDataServices
        {
            private string connectionString = "Data Source=localhost\\SQLEXPRESS; Initial Catalog = LPSystem; Integrated Security = True; TrustServerCertificate=True;";

            private SqlConnection sqlConnection;

            public LPDBData()
            {
                sqlConnection = new SqlConnection(connectionString);
            }

            public void AddTransaction(Customer customer, string transaction)
            {
                string insertStatement = "INSERT INTO LoyaltyTransactions (PassportId, TransactionDescription) VALUES (@PassportId, @TransactionDescription)";

                SqlCommand command = new SqlCommand(insertStatement, sqlConnection);
                command.Parameters.AddWithValue("@PassportId", customer.PassportId);
                command.Parameters.AddWithValue("@TransactionDescription", transaction);

                sqlConnection.Open();
                command.ExecuteNonQuery();
                sqlConnection.Close();
            }

            public void SaveCustomer(Customer customer)
            {
                string insertStatement = "INSERT INTO Customers (PassportId, CustomerName, LoyaltyPoints) VALUES (@PassportId, @CustomerName, @LoyaltyPoints)";

                SqlCommand command = new SqlCommand(insertStatement, sqlConnection);
                command.Parameters.AddWithValue("@PassportId", customer.PassportId);
                command.Parameters.AddWithValue("@CustomerName", customer.CustomerName);
                command.Parameters.AddWithValue("@LoyaltyPoints", customer.LoyaltyPoints);

                sqlConnection.Open();
                command.ExecuteNonQuery();
                sqlConnection.Close();
            }

            public Customer? GetCustomerByPassportId(string passportId)
            {
                string selectStatement = "SELECT PassportId, CustomerName, LoyaltyPoints FROM Customers WHERE PassportId = @PassportId";

                SqlCommand command = new SqlCommand(selectStatement, sqlConnection);
                command.Parameters.AddWithValue("@PassportId", passportId);

                sqlConnection.Open();
                SqlDataReader reader = command.ExecuteReader();

                Customer? customer = null;

                if (reader.Read())
                {
                    customer = new Customer
                    {
                        PassportId = reader["PassportId"].ToString(),
                        CustomerName = reader["CustomerName"].ToString(),
                        LoyaltyPoints = (int)reader["LoyaltyPoints"],
                        TransactionHistory = GetTransactionsByPassportId(passportId)
                    };
                }

                sqlConnection.Close();
                return customer;
            }

            public List<Customer> GetCustomers()
            {
                string selectStatement = "SELECT PassportId, CustomerName, LoyaltyPoints FROM Customers";

                SqlCommand command = new SqlCommand(selectStatement, sqlConnection);

                sqlConnection.Open();
                SqlDataReader reader = command.ExecuteReader();

                List<Customer> customers = new List<Customer>();

                while (reader.Read())
                {
                    customers.Add(new Customer
                    {
                        PassportId = reader["PassportId"].ToString(),
                        CustomerName = reader["CustomerName"].ToString(),
                        LoyaltyPoints = (int)reader["LoyaltyPoints"],
                        TransactionHistory = new List<string>()
                    });
                }

                sqlConnection.Close();
                return customers;
            }

            public void UpdateCustomer(Customer customer)
            {
                string updateStatement = "UPDATE Customers SET CustomerName = @CustomerName, LoyaltyPoints = @LoyaltyPoints WHERE PassportId = @PassportId";

                SqlCommand command = new SqlCommand(updateStatement, sqlConnection);
                command.Parameters.AddWithValue("@CustomerName", customer.CustomerName);
                command.Parameters.AddWithValue("@LoyaltyPoints", customer.LoyaltyPoints);
                command.Parameters.AddWithValue("@PassportId", customer.PassportId);

                sqlConnection.Open();
                command.ExecuteNonQuery();
                sqlConnection.Close();
            }

            private List<string> GetTransactionsByPassportId(string passportId)
            {
                List<string> transactions = new List<string>();

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string selectStatement = "SELECT TransactionDescription FROM LoyaltyTransactions WHERE PassportId = @PassportId";

                    SqlCommand command = new SqlCommand(selectStatement, connection);
                    command.Parameters.AddWithValue("@PassportId", passportId);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        transactions.Add(reader["TransactionDescription"].ToString());
                    }
                }

                return transactions;
            }
        }
    
}


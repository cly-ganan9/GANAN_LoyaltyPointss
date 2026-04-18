using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using LoyaltyPointsModels;
using Microsoft.Data.SqlClient;

namespace LoyaltyPointsDataServices
{
    public class LPDBData : ILPDataServices
    {
        private readonly string connectionString =
            "Data Source=localhost\\SQLEXPRESS; Initial Catalog=LPSystem; Integrated Security=True; TrustServerCertificate=True;";

        // Transactions are stored as JSON string in the DB column for simplicity
        public void SaveAccount(Account account)
        {
            string sql = "INSERT INTO Accounts (AccountId, FirstName, LastName, Birthdate, Username, Password, LoyaltyPoints, Transactions) " +
                         "VALUES (@AccountId, @FirstName, @LastName, @Birthdate, @Username, @Password, @LoyaltyPoints, @Transactions)";

            using var conn = new SqlConnection(connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@AccountId", account.AccountId);
            cmd.Parameters.AddWithValue("@FirstName", account.FirstName);
            cmd.Parameters.AddWithValue("@LastName", account.LastName);
            cmd.Parameters.AddWithValue("@Birthdate", account.Birthdate);
            cmd.Parameters.AddWithValue("@Username", account.Username);
            cmd.Parameters.AddWithValue("@Password", account.Password);
            cmd.Parameters.AddWithValue("@LoyaltyPoints", account.LoyaltyPoints);
            cmd.Parameters.AddWithValue("@Transactions", JsonSerializer.Serialize(account.Transactions));
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public Account? GetAccountByUsername(string username)
        {
            string sql = "SELECT * FROM Accounts WHERE Username = @Username";

            using var conn = new SqlConnection(connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Username", username);
            conn.Open();
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
                return MapAccount(reader);

            return null;
        }

        public Account? GetAccountById(string accountId)
        {
            string sql = "SELECT * FROM Accounts WHERE AccountId = @AccountId";

            using var conn = new SqlConnection(connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@AccountId", accountId);
            conn.Open();
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
                return MapAccount(reader);

            return null;
        }

        public List<Account> GetAllAccounts()
        {
            string sql = "SELECT * FROM Accounts";
            var accounts = new List<Account>();

            using var conn = new SqlConnection(connectionString);
            using var cmd = new SqlCommand(sql, conn);
            conn.Open();
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
                accounts.Add(MapAccount(reader));

            return accounts;
        }

        public void UpdateAccount(Account account)
        {
            string sql = "UPDATE Accounts SET FirstName=@FirstName, LastName=@LastName, Birthdate=@Birthdate, " +
                         "Username=@Username, Password=@Password, LoyaltyPoints=@LoyaltyPoints, Transactions=@Transactions " +
                         "WHERE AccountId=@AccountId";

            using var conn = new SqlConnection(connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@FirstName", account.FirstName);
            cmd.Parameters.AddWithValue("@LastName", account.LastName);
            cmd.Parameters.AddWithValue("@Birthdate", account.Birthdate);
            cmd.Parameters.AddWithValue("@Username", account.Username);
            cmd.Parameters.AddWithValue("@Password", account.Password);
            cmd.Parameters.AddWithValue("@LoyaltyPoints", account.LoyaltyPoints);
            cmd.Parameters.AddWithValue("@Transactions", JsonSerializer.Serialize(account.Transactions));
            cmd.Parameters.AddWithValue("@AccountId", account.AccountId);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void DeleteAccount(string accountId)
        {
            string sql = "DELETE FROM Accounts WHERE AccountId = @AccountId";

            using var conn = new SqlConnection(connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@AccountId", accountId);
            conn.Open();
            cmd.ExecuteNonQuery();
        }

        public void DeleteTransaction(string accountId, int transactionId)
        {
            var account = GetAccountById(accountId);
            if (account != null)
            {
                var t = account.Transactions.FirstOrDefault(t => t.TransactionId == transactionId);
                if (t != null)
                {
                    account.Transactions.Remove(t);
                    UpdateAccount(account);
                }
            }
        }

        private Account MapAccount(SqlDataReader reader)
        {
            string transJson = reader["Transactions"].ToString() ?? "[]";
            var transactions = JsonSerializer.Deserialize<List<Transaction>>(transJson) ?? new List<Transaction>();

            return new Account
            {
                AccountId = reader["AccountId"].ToString()!,
                FirstName = reader["FirstName"].ToString()!,
                LastName = reader["LastName"].ToString()!,
                Birthdate = reader["Birthdate"].ToString()!,
                Username = reader["Username"].ToString()!,
                Password = reader["Password"].ToString()!,
                LoyaltyPoints = (int)reader["LoyaltyPoints"],
                Transactions = transactions
            };
        }
    }
}

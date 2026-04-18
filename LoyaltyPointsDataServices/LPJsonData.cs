using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using LoyaltyPointsModels;

namespace LoyaltyPointsDataServices
{
    public class LPJsonData : ILPDataServices
    {
        private List<Account> accounts = new List<Account>();
        private readonly string _jsonFileName;

        public LPJsonData()
        {
            _jsonFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Accounts.json");
            LoadFromFile();
        }
        private void SaveToFile()
        {
            string json = JsonSerializer.Serialize(accounts, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_jsonFileName, json);
        }

        private void LoadFromFile()
        {
            if (!File.Exists(_jsonFileName))
            {
                accounts = new List<Account>();
                return;
            }

            string json = File.ReadAllText(_jsonFileName);
            accounts = JsonSerializer.Deserialize<List<Account>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new List<Account>();
        }

        public void SaveAccount(Account account)
        {
            LoadFromFile();
            var existing = GetAccountByUsername(account.Username);
            if (existing == null)
            {
                accounts.Add(account);
                SaveToFile();
            }
        }

        public Account? GetAccountByUsername(string username)
        {
            return accounts.FirstOrDefault(a => a.Username == username);
        }

        public Account? GetAccountById(string accountId)
        {
            return accounts.FirstOrDefault(a => a.AccountId == accountId);
        }

        public List<Account> GetAllAccounts()
        {
            LoadFromFile();
            return accounts;
        }

        public void UpdateAccount(Account account)
        {
            LoadFromFile();
            var existing = accounts.FirstOrDefault(a => a.AccountId == account.AccountId);
            if (existing != null)
            {
                existing.FirstName = account.FirstName;
                existing.LastName = account.LastName;
                existing.Birthdate = account.Birthdate;
                existing.Username = account.Username;
                existing.Password = account.Password;
                existing.LoyaltyPoints = account.LoyaltyPoints;
                existing.Transactions = account.Transactions;
                SaveToFile();
            }
        }

        public void DeleteAccount(string accountId)
        {
            LoadFromFile();
            var existing = accounts.FirstOrDefault(a => a.AccountId == accountId);
            if (existing != null)
            {
                accounts.Remove(existing);
                SaveToFile();
            }
        }

        public void DeleteTransaction(string accountId, int transactionId)
        {
            LoadFromFile();
            var account = accounts.FirstOrDefault(a => a.AccountId == accountId);
            if (account != null)
            {
                var transaction = account.Transactions.FirstOrDefault(t => t.TransactionId == transactionId);
                if (transaction != null)
                {
                    account.Transactions.Remove(transaction);
                    SaveToFile();
                }
            }
        }
    }
}

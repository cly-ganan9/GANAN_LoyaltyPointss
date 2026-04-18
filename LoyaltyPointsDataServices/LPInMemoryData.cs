using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoyaltyPointsModels;

namespace LoyaltyPointsDataServices
{
    internal class LPInMemoryData : ILPDataServices
    {
        private List<Account> accounts = new List<Account>();

        public void SaveAccount(Account account)
        {
            var existing = GetAccountByUsername(account.Username);
            if (existing == null)
                accounts.Add(account);
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
            return accounts;
        }

        public void UpdateAccount(Account account)
        {
            var existing = GetAccountById(account.AccountId);
            if (existing != null)
            {
                existing.FirstName = account.FirstName;
                existing.LastName = account.LastName;
                existing.Birthdate = account.Birthdate;
                existing.Username = account.Username;
                existing.Password = account.Password;
                existing.LoyaltyPoints = account.LoyaltyPoints;
                existing.Transactions = account.Transactions;
            }
        }

        public void DeleteAccount(string accountId)
        {
            var existing = GetAccountById(accountId);
            if (existing != null)
                accounts.Remove(existing);
        }

        public void DeleteTransaction(string accountId, int transactionId)
        {
            var account = GetAccountById(accountId);
            if (account != null)
            {
                var transaction = account.Transactions.FirstOrDefault(t => t.TransactionId == transactionId);
                if (transaction != null)
                    account.Transactions.Remove(transaction);
            }
        }
    }
}

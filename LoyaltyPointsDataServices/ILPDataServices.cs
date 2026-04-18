using System.Collections.Generic;
using LoyaltyPointsModels;

namespace LoyaltyPointsDataServices
{
    public interface ILPDataServices
    {
        // Account CRUD
        void SaveAccount(Account account);
        Account? GetAccountByUsername(string username);
        Account? GetAccountById(string accountId);
        List<Account> GetAllAccounts();
        void UpdateAccount(Account account);
        void DeleteAccount(string accountId);

        // Transaction
        void DeleteTransaction(string accountId, int transactionId);
    }
}

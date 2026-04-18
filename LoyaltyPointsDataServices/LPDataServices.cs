using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoyaltyPointsModels;

namespace LoyaltyPointsDataServices
{
    public class LPDataServices
    {
        private readonly ILPDataServices _dataService;

        public LPDataServices(ILPDataServices dataService) => _dataService = dataService;

        public void SaveAccount(Account account) => _dataService.SaveAccount(account);
        public Account? GetAccountByUsername(string username) => _dataService.GetAccountByUsername(username);
        public Account? GetAccountById(string accountId) => _dataService.GetAccountById(accountId);
        public List<Account> GetAllAccounts() => _dataService.GetAllAccounts();
        public void UpdateAccount(Account account) => _dataService.UpdateAccount(account);
        public void DeleteAccount(string accountId) => _dataService.DeleteAccount(accountId);
        public void DeleteTransaction(string accountId, int transactionId) => _dataService.DeleteTransaction(accountId, transactionId);
    }
}


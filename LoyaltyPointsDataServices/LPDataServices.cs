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
        public void AddTransaction(Customer customer, string transaction)
        {
            customer.TransactionHistory.Add(transaction);
        }
    }
}

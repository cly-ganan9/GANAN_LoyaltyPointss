using System;
using System.Collections.Generic;

namespace LoyaltyPointsModels
{
    public class Customer
    {
        public string PassportId { get; set; }
        public string CustomerName { get; set; }
        public int LoyaltyPoints { get; set; }
        public List<string> TransactionHistory { get; set; }

        public Customer()
        {
            TransactionHistory = new List<string>();
        }
    }
}

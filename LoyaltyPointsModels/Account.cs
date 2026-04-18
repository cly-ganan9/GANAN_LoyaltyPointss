using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyPointsModels
{
    public class Account
    {
        public string AccountId { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Birthdate { get; set; } = string.Empty;   // stored as "mm/dd/yyyy"
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int LoyaltyPoints { get; set; }
        public List<Transaction> Transactions { get; set; }

        public Account()
        {
            AccountId = Guid.NewGuid().ToString();
            Transactions = new List<Transaction>();
            LoyaltyPoints = 0;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoyaltyPointsModels
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public string Type { get; set; } = string.Empty;        // "Earn" or "Redeem"
        public int Points { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; }

        public Transaction()
        {
            Date = DateTime.Now;
        }
    }
}

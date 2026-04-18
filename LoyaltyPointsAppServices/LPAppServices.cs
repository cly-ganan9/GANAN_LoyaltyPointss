using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoyaltyPointsModels;
using LoyaltyPointsDataServices;

namespace LoyaltyPointsAppServices
{
    // Business layer: handles all app logic, rules, and calculations.
    // The UI calls this — never the data layer directly.
    public class LPAppServices
    {
        private readonly LPDataServices _data;

        public LPAppServices(LPDataServices dataServices)
        {
            _data = dataServices;
        }
        public List<Flight> GetFlights()
        {
            return new List<Flight>
            {
                new Flight { FlightId = 112, Destination = "Manila, Philippines to Seoul, South Korea", RoundTripPrice = 16199 },
                new Flight { FlightId = 234, Destination = "Cebu, Philippines to Tokyo, Japan",         RoundTripPrice = 21592 },
                new Flight { FlightId = 156, Destination = "Manila, Philippines to Bangkok, Thailand",  RoundTripPrice = 23208 },
                new Flight { FlightId = 278, Destination = "Cebu, Philippines to Davao, Philippines",  RoundTripPrice = 5289  },
            };
        }

        public List<Reward> GetRewards()
        {
            return new List<Reward>
            {
                new Reward { RewardId = 1, Name = "Flight Discount",  PointsCost = 500,  Description = "Redeemed for 20% flight discount" },
                new Reward { RewardId = 2, Name = "Seat Upgrade",     PointsCost = 300,  Description = "Redeemed for one seat upgrade" },
                new Reward { RewardId = 3, Name = "Extra Baggage",    PointsCost = 200,  Description = "Redeemed for extra baggage allowance" },
                new Reward { RewardId = 4, Name = "Travel Voucher",   PointsCost = 1000, Description = "Redeemed for travel voucher" },
                new Reward { RewardId = 5, Name = "Morning Coffee",   PointsCost = 100,  Description = "Redeemed for morning coffee" },
            };
        }

        public Flight? GetFlightById(int flightId)
        {
            return GetFlights().FirstOrDefault(f => f.FlightId == flightId);
        }

        public void RegisterAccount(Account account)
        {
            _data.SaveAccount(account);
        }
        public Account? Login(string username, string password)
        {
            var account = _data.GetAccountByUsername(username);
            if (!Checks.IsLoginValid(username, password, account))
                return null;
            return account;
        }

        public List<Account> GetAllAccounts()
        {
            return _data.GetAllAccounts();
        }
        public int GetClassMultiplier(int classOption)
        {
            return classOption switch
            {
                1 => 5,
                2 => 4,
                3 => 3,
                4 => 2,
                _ => 0
            };
        }

        public string GetClassName(int classOption)
        {
            return classOption switch
            {
                1 => "First Class",
                2 => "Business Class",
                3 => "Premium Class",
                4 => "Economy Class",
                _ => "Unknown"
            };
        }

        // Formula: (RoundTripPrice / 50) * classMultiplier = points earned
        public int CalculatePoints(decimal roundTripPrice, int classMultiplier)
        {
            return (int)(roundTripPrice / 50) * classMultiplier;
        }

        public void EarnPoints(Account account, Flight flight, int classOption)
        {
            int multiplier = GetClassMultiplier(classOption);
            int earned = CalculatePoints(flight.RoundTripPrice, multiplier);
            string className = GetClassName(classOption);

            account.LoyaltyPoints += earned;

            int newId = account.Transactions.Count > 0
                ? account.Transactions.Max(t => t.TransactionId) + 1
                : 1;

            account.Transactions.Add(new Transaction
            {
                TransactionId = newId,
                Type = "Earn",
                Points = earned,
                Description = $"Flight ID: {flight.FlightId} - {className}",
                Date = DateTime.Now
            });

            _data.UpdateAccount(account);
        }

        // Redeem Points

        public bool RedeemPoints(Account account, Reward reward)
        {
            if (!Checks.HasEnoughPoints(account, reward.PointsCost))
                return false;

            account.LoyaltyPoints -= reward.PointsCost;

            int newId = account.Transactions.Count > 0
                ? account.Transactions.Max(t => t.TransactionId) + 1
                : 1;

            account.Transactions.Add(new Transaction
            {
                TransactionId = newId,
                Type = "Redeem",
                Points = reward.PointsCost,
                Description = reward.Description,
                Date = DateTime.Now
            });

            _data.UpdateAccount(account);
            return true;
        }
        public void UpdateAccount(Account account)
        {
            _data.UpdateAccount(account);
        }

        public bool DeleteTransaction(Account account, int transactionId)
        {
            if (!Checks.TransactionExists(account, transactionId))
                return false;

            var t = account.Transactions.First(t => t.TransactionId == transactionId);

            // Reverse the points effect
            if (t.Type == "Earn")
                account.LoyaltyPoints -= t.Points;
            else if (t.Type == "Redeem")
                account.LoyaltyPoints += t.Points;

            account.Transactions.Remove(t);
            _data.UpdateAccount(account);
            return true;
        }

        public bool DeleteAccount(Account account, string username, string password)
        {
            if (!Checks.IsDeleteCredentialValid(username, password, account))
                return false;

            _data.DeleteAccount(account.AccountId);
            return true;
        }

        public int GetTotalPointsEarned(Account account)
        {
            int total = 0;
            foreach (var t in account.Transactions)
                if (t.Type == "Earn") total += t.Points;
            return total;
        }

        public int GetTotalPointsRedeemed(Account account)
        {
            int total = 0;
            foreach (var t in account.Transactions)
                if (t.Type == "Redeem") total += t.Points;
            return total;
        }

        public DateTime? GetLastTransactionDate(Account account)
        {
            if (account.Transactions.Count == 0) return null;
            return account.Transactions[account.Transactions.Count - 1].Date;
        }
    }
}

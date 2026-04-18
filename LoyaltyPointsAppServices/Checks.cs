using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoyaltyPointsModels;

namespace LoyaltyPointsAppServices
{
    // Checks: business rule checks (does username exist? enough points? correct password?)
    public static class Checks
    {
        // Returns true if the username is already taken
        public static bool IsUsernameTaken(string username, List<Account> allAccounts)
        {
            return allAccounts.Exists(a => a.Username == username);
        }

        // Returns true if login credentials match
        public static bool IsLoginValid(string username, string password, Account? account)
        {
            if (account == null) return false;
            return account.Username == username && account.Password == password;
        }

        // Returns true if account has enough points to redeem
        public static bool HasEnoughPoints(Account account, int requiredPoints)
        {
            return account.LoyaltyPoints >= requiredPoints;
        }

        // Returns true if the flight ID exists in the list
        public static bool IsFlightValid(int flightId, List<Flight> flights)
        {
            return flights.Exists(f => f.FlightId == flightId);
        }

        // Returns true if a transaction with that ID exists for this account
        public static bool TransactionExists(Account account, int transactionId)
        {
            return account.Transactions.Exists(t => t.TransactionId == transactionId);
        }

        // Returns true if delete credentials match the logged-in account
        public static bool IsDeleteCredentialValid(string username, string password, Account account)
        {
            return account.Username == username && account.Password == password;
        }
    }
}

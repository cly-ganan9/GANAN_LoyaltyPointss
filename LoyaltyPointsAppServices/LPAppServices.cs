using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoyaltyPointsModels;
using LoyaltyPointsDataServices;

namespace LoyaltyPointsAppServices
{
    public class LPAppServices
    {
        private LPDataServices dataServices = new LPDataServices();

        public int EarnPoints(Customer customer, string destination, decimal ticketPrice)
        {
            int earnedPoints = (int)(ticketPrice / 100);
            customer.LoyaltyPoints += earnedPoints;

            dataServices.AddTransaction(customer, "Earned " + earnedPoints + " points for flight to " + destination);
            dataServices.UpdateCustomer(customer);

            return earnedPoints;
        }

        public (string rewardName, int rewardCost) RedeemPoints(Customer customer, int option)
        {
            string[] rewardNames = { "Starbucks Voucher", "50% Travel Discount", "Lounge Access", "SM Gift Card" };
            int[] rewardPoints = { 100, 500, 1000, 200 };

            int index = option - 1;
            customer.LoyaltyPoints -= rewardPoints[index];

            dataServices.AddTransaction(customer, "Redeemed " + rewardNames[index] + " for " + rewardPoints[index] + " points");
            dataServices.UpdateCustomer(customer);

            return (rewardNames[index], rewardPoints[index]);
        }

        public Customer ViewAccount(Customer customer)
        {
            return customer;
        }
        public string[] GetRewardNames()
        {
            return new string[] { "Starbucks Voucher", "50% Travel Discount", "Lounge Access", "SM Gift Card" };
        }
        public int[] GetRewardPoints()
        {
            return new int[] { 100, 500, 1000, 200 };
        }
        public void SaveCustomer(Customer customer)
        {
            dataServices.SaveCustomer(customer);
        }
    }
}

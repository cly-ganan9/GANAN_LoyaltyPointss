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
        LPDataServices dataServices = new LPDataServices();

        public void EarnPoints(Customer customer)
        {
            Console.WriteLine("--------- EARN POINTS ---------");
            
            Console.Write("Enter Destination: ");
            string destination = Console.ReadLine().ToUpper();

            Console.Write("Enter Ticket Price: ");
            decimal ticketPrice = Convert.ToDecimal(Console.ReadLine());

            int earnedPoints = (int)(ticketPrice / 100);
            customer.LoyaltyPoints += earnedPoints;

            Console.WriteLine();
            Console.WriteLine("Points Earned: " + earnedPoints);
            Console.WriteLine("Updated Balance: " + customer.LoyaltyPoints);

            dataServices.AddTransaction(customer, "Earned " + earnedPoints + " points from " + destination);
        }

        public void RedeemPoints(Customer customer)
        {
            Console.WriteLine("--------- REDEEM POINTS ---------");
            Console.WriteLine("Current Points: " + customer.LoyaltyPoints);
            Console.WriteLine();
            Console.Write("Choose reward to redeem (1-4): ");
            int options = Convert.ToInt16(Console.ReadLine());

            string[] rewardNames = { "Starbucks Voucher", "50% Travel Discount", "Lounge Access", "SM Gift Card" };
            int[] rewardPoints = { 100, 500, 1000, 200 };

            for (int i = 0; i < rewardNames.Length; i++)
            {
                Console.WriteLine((i + 1) + ". " + rewardNames[i] + " - " + rewardPoints[i] + " points");
            }

            int index = options - 1;
            customer.LoyaltyPoints -= rewardPoints[index];

            Console.WriteLine();
            Console.WriteLine("Redeemed: " + rewardNames[index]);
            Console.WriteLine("Updated Balance: " + customer.LoyaltyPoints);

            dataServices.AddTransaction(customer, "Redeemed " + rewardNames[index] + " for " + rewardPoints[index] + " points");


        }

        public void ViewAccount(Customer customer)
        {
            Console.WriteLine("--------- ACCOUNT DETAILS ---------");

            Console.WriteLine("Passport ID: " + customer.PassportId);
            Console.WriteLine("Customer Name: " + customer.CustomerName);
            Console.WriteLine("Current Points: " + customer.LoyaltyPoints);

            Console.WriteLine();
            Console.WriteLine("Transaction History:");

            if (customer.TransactionHistory.Count == 0)
            {
                Console.WriteLine("No transactions yet.");
            }
            else
            {
                for (int i = 0; i < customer.TransactionHistory.Count; i++)
                {
                    Console.WriteLine((i + 1) + ". " + customer.TransactionHistory[i]);
                }

            }
        }    
    }
}

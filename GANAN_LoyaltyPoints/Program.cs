using System;
using System.Collections.Generic;
using LoyaltyPointsModels;
using LoyaltyPointsAppServices;


namespace GANAN_LoyaltyPoints
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Flight Booking App: Loyalty Points System";

            LPAppServices appServices = new LPAppServices();
            Customer customer = new Customer();

            Console.WriteLine("-------------------------------");
            Console.WriteLine("        Loyalty Points");
            Console.WriteLine("-------------------------------");

            Console.Write("Enter Passport ID: ");
            customer.PassportId = Console.ReadLine();

            Console.Write("Enter Customer Name: ");
            customer.CustomerName = Console.ReadLine().ToUpper();

            Console.Write("Enter Current Loyalty Points: ");
            customer.LoyaltyPoints = Convert.ToInt32(Console.ReadLine());

            appServices.SaveCustomer(customer);

            bool running = true;

            while (running) 
            {
                Console.WriteLine("-------------------------------");
                Console.WriteLine("            MENU");
                Console.WriteLine("-------------------------------");
                Console.WriteLine("1. Earn Points");
                Console.WriteLine("2. Redeem Points");
                Console.WriteLine("3. View Account");
                Console.WriteLine("4. Exit");
                Console.WriteLine("-------------------------------");
                Console.Write("Choose an option (1-4): ");

                int choice = Convert.ToInt16(Console.ReadLine());

                if (choice == 1)
                {
                    Console.WriteLine("--------- EARN POINTS ---------");

                    Console.Write("Enter Destination: ");
                    string destination = Console.ReadLine().ToUpper();

                    Console.Write("Enter Ticket Price: ");
                    decimal ticketPrice = Convert.ToDecimal(Console.ReadLine());

                    int earnedPoints = appServices.EarnPoints(customer, destination, ticketPrice);

                    Console.WriteLine();
                    Console.WriteLine("Points Earned: " + earnedPoints);
                    Console.WriteLine("Updated Balance: " + customer.LoyaltyPoints);
                }
                else if (choice == 2)
                {
                    Console.WriteLine("--------- REDEEM POINTS ---------");
                    Console.WriteLine("Current Points: " + customer.LoyaltyPoints);
                    Console.WriteLine();

                    string[] rewardNames = appServices.GetRewardNames();
                    int[] rewardPoints = appServices.GetRewardPoints();

                    for (int i = 0; i < rewardNames.Length; i++)
                    {
                        Console.WriteLine((i + 1) + ". " + rewardNames[i] + " - " + rewardPoints[i] + " points");
                    }

                    Console.Write("Choose reward to redeem (1-4): ");
                    int option = Convert.ToInt16(Console.ReadLine());

                    var redeemed = appServices.RedeemPoints(customer, option);

                    Console.WriteLine();
                    Console.WriteLine("Redeemed: " + redeemed.rewardName);
                    Console.WriteLine("Updated Balance: " + customer.LoyaltyPoints);
                }
                else if (choice == 3)
                {
                    Customer account = appServices.ViewAccount(customer);

                    Console.WriteLine("--------- ACCOUNT DETAILS ---------");
                    Console.WriteLine("Passport ID: " + account.PassportId);
                    Console.WriteLine("Customer Name: " + account.CustomerName);
                    Console.WriteLine("Current Points: " + account.LoyaltyPoints);

                    Console.WriteLine();
                    Console.WriteLine("Transaction History:");

                    if (account.TransactionHistory.Count == 0)
                    {
                        Console.WriteLine("No transactions yet.");
                    }
                    else
                    {
                        for (int i = 0; i < account.TransactionHistory.Count; i++)
                        {
                            Console.WriteLine((i + 1) + ". " + account.TransactionHistory[i]);
                        }
                    }
                }
                else if (choice == 4)
                {
                    running = false;
                    Console.WriteLine("END OF TRANSACTION");
                }
                else
                {
                    Console.WriteLine("Invalid option. Please choose between 1 and 4.");
                }

                Console.WriteLine();
                Console.WriteLine("Press enter to continue...");
                Console.ReadKey();
                
            }
            
        }

    }

}
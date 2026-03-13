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
                    appServices.EarnPoints(customer);
                }
                else if (choice == 2)
                {
                    appServices.RedeemPoints(customer);
                }
                else if (choice == 3)
                {
                    appServices.ViewAccount(customer);
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
namespace GANAN_LoyaltyPoints
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Philippine Airlines!\n**Loyalty Points Section**");

            string username, flightCode = "PH1234";
            int points = 0, phcode = 20;

            Console.Write("Enter username: ");
            string usernameInp = Console.ReadLine();

            Console.WriteLine(usernameInp);
          

            Console.WriteLine("Available Points: "+points+"pts");

            Console.Write("Proceeed to Add Points or Redeem Points? (add/redeem): ";
            string choice = Console.ReadLine();

            if (choice == "add")
            {
                Console.Write("**ADD POINTS**\nEnter flight code: ");
                string code = Console.ReadLine();

                if (code == flightCode)
                {
                    int addCode = points + phcode;
                    Console.Write("Available Points: " + addCode);
                }
                else
                {
                    Console.WriteLine("Not Available");
                }
                
            }
            else if (choice == "redeem")
            {
                Console.WriteLine("**REDEEM POINTS**");
                Console.WriteLine("1) 10% OFF Starbucks\n2) Travel Voucher to Korea\n Choose rewards: ");
            }

        }
    }
}

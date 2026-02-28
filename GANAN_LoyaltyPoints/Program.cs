namespace GANAN_LoyaltyPoints
{
    internal class Program
    {
        static void Main(string[] args)
        {
            
string customerName = "", passportId = "", redeemRewards, destination;
            int points = 0, earnedPointsList = 0, earnedCount = 0, redeemCount = 0, menuInp;
            decimal ticketPrice = 0.0m;
            
            Console.WriteLine("Welcome to Philippine Airlines!\n------Loyalty Points Section------\n");

            Console.Write("Enter Passport ID: ");
            passportId = Console.ReadLine();
                if  (passportId == null);
                passportId = passportId.Trim().ToUpper();

            Console.Write("Enter Customer Name: ");
            customerName = Console.ReadLine();
                if (customerName == null);
                customerName = customerName.Trim().ToUpper();

            Console.Write("Enter Current Points: ");
            points = Convert.ToInt32(Console.ReadLine());
           if (points < 0)
            {
                Console.WriteLine("ERROR: Loyalty points must not be less than 0.");
            }
            else
            {
                 Console.WriteLine("ERROR: Loyalty points must be a number.");
                
            }

            bool menu = true;
             while (menu)
            {
                Console.WriteLine("\n------MENU------");
                Console.WriteLine("1) Earn Points\n2) Redeem Points\n3) View Account\n 4) Exit");
                Console.Write("Choose 1-4: ");
                menuInp = Convert.ToInt32(Console.ReadLine());
                if (menuInp < 1 || menuInp > 4)
                {
                    Console.WriteLine("ERROR: Choose 1 to 4 only."); 
                    return;
                }
                else if (menuInp == 1)
                {
                    Console.WriteLine("------EARN POINTS------");
                    Console.Write("Enter Destination: ");
                    destination = Console.ReadLine();
                    if (destination == null) ;
                    destination = destination.Trim().ToUpper();
                }
                else
                {
                    Console.WriteLine("ERROR: Numbers Only");
                    return;
                }

        }
    }
}

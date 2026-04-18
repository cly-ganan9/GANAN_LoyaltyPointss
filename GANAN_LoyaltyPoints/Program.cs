using System;
using System.Collections.Generic;
using LoyaltyPointsAppServices;
using LoyaltyPointsDataServices;
using LoyaltyPointsModels;

namespace GANAN_LoyaltyPoints
{
    internal class Program
    {
        static LPAppServices appServices = new LPAppServices(
            new LPDataServices(new LPDBData())
        );

        static void Main(string[] args)
        {
            Console.Title = "Flight Loyalty Points System";
            SeedDefaultAccount(); // Load the pre-stored account from the PDF

            bool running = true;
            while (running)
            {
                ShowMainLoginMenu();
                string option = Console.ReadLine()?.Trim() ?? "";

                if (option == "1")
                    LoginFlow();
                else if (option == "2")
                    CreateAccountFlow();
                else if (option == "0")
                {
                    Console.WriteLine("\nEXITING SYSTEM...");
                    running = false;
                }
                else
                    ShowError("Invalid option. Please enter 1, 2, or 0.");
            }
        }

        static void SeedDefaultAccount()
        {
            var existing = appServices.GetAllAccounts()
                .Find(a => a.Username == "clyza21");

            if (existing == null)
            {
                var account = new Account
                {
                    AccountId = "a1b2c3d4-e5f6-7890-abcd-ef1234567890",
                    FirstName = "Clyza",
                    LastName = "Ganan",
                    Birthdate = "09/21/2006",
                    Username = "clyza21",
                    Password = "cly0921!",
                    LoyaltyPoints = 800
                };
                appServices.RegisterAccount(account);
            }
        }

        static void ShowMainLoginMenu()
        {
            Console.WriteLine();
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("         FLIGHT LOYALTY POINTS SYSTEM");
            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("Log In or Create an Account");
            Console.WriteLine("1. Log In");
            Console.WriteLine("2. Create Account");
            Console.WriteLine("0. Exit");
            Console.Write("Enter option: ");
        }

        static void LoginFlow()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("---------- LOG IN ----------");

                Console.Write("Enter Username: ");
                string username = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Enter Password: ");
                string password = Console.ReadLine()?.Trim() ?? "";

                if (Validations.IsEmpty(username) || Validations.IsEmpty(password))
                {
                    ShowError("Username and password cannot be empty.");
                    continue;
                }

                var account = appServices.Login(username, password);

                if (account == null)
                {
                    ShowError("Invalid username or password. Please try again.");
                    continue;
                }

                Console.WriteLine("\nYou have logged in successfully.");
                MainMenuFlow(account);
                break;
            }
        }

        static void CreateAccountFlow()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("---------- CREATE ACCOUNT ----------");
                Console.WriteLine("Provide account details");

                Console.Write("First Name: ");
                string firstName = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Last Name: ");
                string lastName = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Birthdate (mm/dd/yyyy): ");
                string birthdate = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Username: ");
                string username = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Password: ");
                string password = Console.ReadLine()?.Trim() ?? "";

                // Validations
                if (Validations.IsEmpty(firstName) || Validations.IsEmpty(lastName) ||
                    Validations.IsEmpty(birthdate) || Validations.IsEmpty(username) ||
                    Validations.IsEmpty(password))
                {
                    ShowError("All fields are required. Please try again.");
                    continue;
                }

                if (!Validations.IsValidDateFormat(birthdate))
                {
                    ShowError("Invalid date format. Use mm/dd/yyyy. Please try again.");
                    continue;
                }

                if (!Validations.IsAtLeast18(birthdate))
                {
                    ShowError("You must be at least 18 years old to create an account.");
                    continue;
                }

                if (Checks.IsUsernameTaken(username, appServices.GetAllAccounts()))
                {
                    ShowError("Username is already taken. Please choose another.");
                    continue;
                }

                Console.Write("\nSave account? (y/n): ");
                string confirm = Console.ReadLine()?.Trim().ToLower() ?? "";

                if (!Validations.IsYesOrNo(confirm))
                {
                    ShowError("Please enter y or n.");
                    continue;
                }

                if (confirm == "n")
                {
                    Console.WriteLine("Account creation cancelled.");
                    break;
                }

                var newAccount = new Account
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Birthdate = birthdate,
                    Username = username,
                    Password = password,
                    LoyaltyPoints = 0
                };

                appServices.RegisterAccount(newAccount);
                Console.WriteLine("Your account has been created successfully.");
                break;
            }
        }

        // Main Menu

        static void MainMenuFlow(Account account)
        {
            bool loggedIn = true;
            while (loggedIn)
            {
                Console.WriteLine();
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine("                  MAIN MENU");
                Console.WriteLine("--------------------------------------------------");
                Console.WriteLine("1. View Account");
                Console.WriteLine("2. Earn Points");
                Console.WriteLine("3. Redeem Points");
                Console.WriteLine("4. View Points Summary");
                Console.WriteLine("5. View Transactions");
                Console.WriteLine("6. Update Account");
                Console.WriteLine("7. Delete Transaction / Account");
                Console.WriteLine("8. Log Out");
                Console.WriteLine("0. Exit");
                Console.Write("Enter option: ");

                string option = Console.ReadLine()?.Trim() ?? "";

                if (option == "1") ViewAccountFlow(account);
                else if (option == "2") EarnPointsFlow(account);
                else if (option == "3") RedeemPointsFlow(account);
                else if (option == "4") ViewPointsSummaryFlow(account);
                else if (option == "5") ViewTransactionsFlow(account);
                else if (option == "6") UpdateAccountFlow(account);
                else if (option == "7") DeleteFlow(account, ref loggedIn);
                else if (option == "8")
                {
                    if (LogOutFlow()) loggedIn = false;
                }
                else if (option == "0")
                {
                    Console.WriteLine("\nEXITING SYSTEM...");
                    Environment.Exit(0);
                }
                else
                    ShowError("Invalid option. Please enter 0-8.");
            }
        }

        static void ViewAccountFlow(Account account)
        {
            Console.WriteLine();
            Console.WriteLine("---------- MAIN MENU > VIEW ACCOUNT ----------");
            Console.WriteLine($"Account ID    : {account.AccountId}");
            Console.WriteLine($"First Name    : {account.FirstName}");
            Console.WriteLine($"Last Name     : {account.LastName}");
            Console.WriteLine($"Birthdate     : {account.Birthdate}");
            Console.WriteLine($"Username      : {account.Username}");
            Console.WriteLine($"Current Points: {account.LoyaltyPoints}");

            AskBackOrExit();
        }

        static void EarnPointsFlow(Account account)
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("---------- MAIN MENU > EARN POINTS ----------");
                Console.Write("Enter Flight ID: ");
                string flightInput = Console.ReadLine()?.Trim() ?? "";

                if (!Validations.IsValidInt(flightInput, out int flightId))
                {
                    ShowError("Invalid Flight ID.");
                    continue;
                }

                var flights = appServices.GetFlights();
                if (!Checks.IsFlightValid(flightId, flights))
                {
                    ShowError("Flight ID not found.");
                    continue;
                }

                var flight = appServices.GetFlightById(flightId)!;
                Console.WriteLine($"\nFlight Found: {flight.Destination}");
                Console.WriteLine($"Round Trip Price: PHP {flight.RoundTripPrice:N2}");

                Console.WriteLine();
                Console.WriteLine("Flight Class Type");
                Console.WriteLine("1. First Class");
                Console.WriteLine("2. Business Class");
                Console.WriteLine("3. Premium Class");
                Console.WriteLine("4. Economy Class");
                Console.Write("Enter option: ");

                string classInput = Console.ReadLine()?.Trim() ?? "";
                if (!Validations.IsValidInt(classInput, out int classOption) ||
                    !Validations.IsValidFlightClassOption(classOption))
                {
                    ShowError("Invalid class option. Please choose 1-4.");
                    continue;
                }

                appServices.EarnPoints(account, flight, classOption);

                Console.WriteLine("\nPoints earned successfully!");
                Console.WriteLine($"Total points earned: {account.Transactions[account.Transactions.Count - 1].Points}");
                Console.WriteLine($"Points balance     : {account.LoyaltyPoints}");

                Console.Write("\nEarn more? (y/n): ");
                string again = Console.ReadLine()?.Trim().ToLower() ?? "";

                if (!Validations.IsYesOrNo(again)) { ShowError("Please enter y or n."); continue; }
                if (again == "n") break;
            }
        }

        static void RedeemPointsFlow(Account account)
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("---------- MAIN MENU > REDEEM POINTS ----------");

                var rewards = appServices.GetRewards();
                foreach (var r in rewards)
                    Console.WriteLine($"{r.RewardId}. {r.Name} - {r.PointsCost} pts");
                Console.WriteLine("0. Back");
                Console.Write("Enter option: ");

                string input = Console.ReadLine()?.Trim() ?? "";
                if (!int.TryParse(input, out int option) || !Validations.IsValidRewardOption(option))
                {
                    ShowError("Invalid option. Please choose 0-5.");
                    continue;
                }

                if (option == 0) break;

                var reward = rewards[option - 1];

                Console.WriteLine();
                Console.WriteLine($"  {reward.Name}");
                Console.WriteLine($"  Points : {reward.PointsCost}");
                Console.WriteLine($"  Description: {reward.Description}");

                if (!Checks.HasEnoughPoints(account, reward.PointsCost))
                {
                    ShowError($"Not enough points. You need {reward.PointsCost} pts but have {account.LoyaltyPoints}.");
                    continue;
                }

                Console.Write("\nProceed to redeem? (y/n): ");
                string confirm = Console.ReadLine()?.Trim().ToLower() ?? "";

                if (!Validations.IsYesOrNo(confirm)) { ShowError("Please enter y or n."); continue; }
                if (confirm == "n") continue;

                appServices.RedeemPoints(account, reward);

                Console.WriteLine("\nRedeemed successfully!");
                Console.WriteLine($"Total points used: {reward.PointsCost}");
                Console.WriteLine($"Points balance   : {account.LoyaltyPoints}");

                Console.Write("\nRedeem more? (y/n): ");
                string again = Console.ReadLine()?.Trim().ToLower() ?? "";

                if (!Validations.IsYesOrNo(again)) { ShowError("Please enter y or n."); continue; }
                if (again == "n") break;
            }
        }
        static void ViewPointsSummaryFlow(Account account)
        {
            Console.WriteLine();
            Console.WriteLine("---------- MAIN MENU > VIEW POINTS SUMMARY ----------");
            Console.WriteLine($"Account ID          : {account.AccountId}");
            Console.WriteLine($"Name                : {account.FirstName} {account.LastName}");
            Console.WriteLine($"Total Points Earned : {appServices.GetTotalPointsEarned(account)}");
            Console.WriteLine($"Total Points Redeemed: {appServices.GetTotalPointsRedeemed(account)}");
            Console.WriteLine($"Current Balance     : {account.LoyaltyPoints}");
            Console.WriteLine($"Total Transactions  : {account.Transactions.Count}");

            var lastDate = appServices.GetLastTransactionDate(account);
            Console.WriteLine($"Last Transaction Date: {(lastDate.HasValue ? lastDate.Value.ToString("MM/dd/yyyy hh:mm tt") : "None")}");

            AskBackOrExit();
        }

        static void ViewTransactionsFlow(Account account)
        {
            Console.WriteLine();
            Console.WriteLine("---------- MAIN MENU > VIEW TRANSACTIONS ----------");
            Console.WriteLine($"Name: {account.FirstName} {account.LastName}");
            Console.WriteLine();

            if (account.Transactions.Count == 0)
            {
                Console.WriteLine("No transactions yet.");
            }
            else
            {
                foreach (var t in account.Transactions)
                    Console.WriteLine($"{t.TransactionId}. {t.Type} - {t.Points} pts - {t.Description}");
            }

            AskBackOrExit();
        }

        static void UpdateAccountFlow(Account account)
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("---------- MAIN MENU > UPDATE ACCOUNT ----------");
                Console.WriteLine("1. First Name");
                Console.WriteLine("2. Last Name");
                Console.WriteLine("3. Birthdate");
                Console.WriteLine("4. Username");
                Console.WriteLine("5. Password");
                Console.WriteLine("0. Back");
                Console.Write("Enter option: ");

                string option = Console.ReadLine()?.Trim() ?? "";

                if (option == "0") break;
                else if (option == "1") UpdateFieldFlow(account, "First Name");
                else if (option == "2") UpdateFieldFlow(account, "Last Name");
                else if (option == "3") UpdateFieldFlow(account, "Birthdate");
                else if (option == "4") UpdateFieldFlow(account, "Username");
                else if (option == "5") UpdateFieldFlow(account, "Password");
                else ShowError("Invalid option. Please choose 0-5.");
            }
        }

        static void UpdateFieldFlow(Account account, string field)
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine($"---------- MAIN MENU > UPDATE ACCOUNT > {field.ToUpper()} ----------");
                Console.Write($"Enter new {field}: ");
                string newValue = Console.ReadLine()?.Trim() ?? "";

                if (Validations.IsEmpty(newValue))
                {
                    ShowError($"{field} cannot be empty.");
                    continue;
                }

                // Extra validation for birthdate and username
                if (field == "Birthdate")
                {
                    if (!Validations.IsValidDateFormat(newValue))
                    {
                        ShowError("Invalid date format. Use mm/dd/yyyy.");
                        continue;
                    }
                    if (!Validations.IsAtLeast18(newValue))
                    {
                        ShowError("Birthdate must be at least 18 years or above.");
                        continue;
                    }
                }

                if (field == "Username" && Checks.IsUsernameTaken(newValue, appServices.GetAllAccounts()))
                {
                    ShowError("Username is already taken.");
                    continue;
                }

                Console.Write("Update? (y/n): ");
                string confirm = Console.ReadLine()?.Trim().ToLower() ?? "";

                if (!Validations.IsYesOrNo(confirm)) { ShowError("Please enter y or n."); continue; }
                if (confirm == "n") break;

                if (field == "First Name") account.FirstName = newValue;
                else if (field == "Last Name") account.LastName = newValue;
                else if (field == "Birthdate") account.Birthdate = newValue;
                else if (field == "Username") account.Username = newValue;
                else if (field == "Password") account.Password = newValue;

                appServices.UpdateAccount(account);
                Console.WriteLine("Account updated successfully!");

                AskBackOrExit();
                break;
            }
        }
        static void DeleteFlow(Account account, ref bool loggedIn)
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("---------- MAIN MENU > DELETE ----------");
                Console.WriteLine("1. Delete Transaction");
                Console.WriteLine("2. Delete Account");
                Console.Write("Enter option: ");

                string option = Console.ReadLine()?.Trim() ?? "";

                if (option == "1")
                {
                    DeleteTransactionFlow(account);
                    break;
                }
                else if (option == "2")
                {
                    bool deleted = DeleteAccountFlow(account);
                    if (deleted) { loggedIn = false; }
                    break;
                }
                else
                    ShowError("Invalid option. Please enter 1 or 2.");
            }
        }

        static void DeleteTransactionFlow(Account account)
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("---------- MAIN MENU > DELETE TRANSACTION ----------");
                Console.Write("Enter Transaction ID: ");
                string input = Console.ReadLine()?.Trim() ?? "";

                if (!Validations.IsValidInt(input, out int transId))
                {
                    ShowError("Invalid Transaction ID.");
                    continue;
                }

                if (!Checks.TransactionExists(account, transId))
                {
                    ShowError("Transaction not found.");
                    continue;
                }

                Console.Write("Delete transaction? (y/n): ");
                string confirm = Console.ReadLine()?.Trim().ToLower() ?? "";

                if (!Validations.IsYesOrNo(confirm)) { ShowError("Please enter y or n."); continue; }
                if (confirm == "n") break;

                appServices.DeleteTransaction(account, transId);
                Console.WriteLine("Transaction deleted successfully!");

                AskBackOrExit();
                break;
            }
        }

        static bool DeleteAccountFlow(Account account)
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("---------- MAIN MENU > DELETE ACCOUNT ----------");
                Console.Write("Enter Username: ");
                string username = Console.ReadLine()?.Trim() ?? "";

                Console.Write("Enter Password: ");
                string password = Console.ReadLine()?.Trim() ?? "";

                if (Validations.IsEmpty(username) || Validations.IsEmpty(password))
                {
                    ShowError("Username and password cannot be empty.");
                    continue;
                }

                Console.Write("Delete account? (y/n): ");
                string confirm = Console.ReadLine()?.Trim().ToLower() ?? "";

                if (!Validations.IsYesOrNo(confirm)) { ShowError("Please enter y or n."); continue; }
                if (confirm == "n") return false;

                bool deleted = appServices.DeleteAccount(account, username, password);

                if (!deleted)
                {
                    ShowError("Incorrect username or password.");
                    continue;
                }

                Console.WriteLine("Account deleted successfully!");
                return true;
            }
        }

        static bool LogOutFlow()
        {
            while (true)
            {
                Console.Write("\nDo you want to log out? (y/n): ");
                string input = Console.ReadLine()?.Trim().ToLower() ?? "";

                if (!Validations.IsYesOrNo(input)) { ShowError("Please enter y or n."); continue; }

                if (input == "y") return true;
                return false;
            }
        }

        // Asks "Back to main menu? (y/n)" — if n, exit the program
        static void AskBackOrExit()
        {
            while (true)
            {
                Console.Write("\nBack to main menu? (y/n): ");
                string input = Console.ReadLine()?.Trim().ToLower() ?? "";

                if (!Validations.IsYesOrNo(input)) { ShowError("Please enter y or n."); continue; }

                if (input == "n")
                {
                    Console.WriteLine("\nEXITING SYSTEM...");
                    Environment.Exit(0);
                }
                break;
            }
        }

        static void ShowError(string message)
        {
            Console.WriteLine($"\n[!] {message}");
        }
    }
}
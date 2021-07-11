using System;
using System.Collections.Generic;
using TenmoClient.Models;
using Figgle;



namespace TenmoClient
{
    class Program
    {
        private static readonly ConsoleService consoleService = new ConsoleService();
        private static readonly AuthService authService = new AuthService();
        private static readonly AccountService acctservice = new AccountService();
        public static ApiTransfer transfer = new ApiTransfer();

        static void Main(string[] args)
        {
            Console.BackgroundColor = ConsoleColor.White;
            Console.Clear();
            Run();
        }

        private static void Run()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            string TenMoHeader = FiggleFonts.Speed.Render("                TENMO");
            int loginRegister = -1;
            while (loginRegister != 1 && loginRegister != 2)
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine(TenMoHeader);
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.CursorLeft = 47;
                Console.WriteLine("Welcome to TEnmo!");
                Console.WriteLine();
                Console.WriteLine();
                Console.CursorLeft = 50;
                Console.WriteLine("1: Login");
                Console.WriteLine();
                Console.CursorLeft = 50;
                Console.WriteLine("2: Register");
                Console.WriteLine();
                Console.CursorLeft = 45;
                Console.Write("Please choose an option: ");


                
                if (!int.TryParse(Console.ReadLine(), out loginRegister))
                {
                    Console.Clear();
                    Console.Beep(440, 200);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();                
                    string warningHeader = FiggleFonts.Standard.Render("                                            Error"); 
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(warningHeader);
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.CursorLeft = 47;
                    Console.WriteLine("Press Enter to continue");
                    Console.ReadLine();
                    Console.Clear();
                }
                else if (loginRegister == 1)
                {

                    Console.Clear();
                    
                    while (!UserService.IsLoggedIn()) //will keep looping until user is logged in
                    {

                       
                        LoginUser loginUser = consoleService.PromptForLogin();
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine();
                        Console.CursorLeft = 30;
                        ApiUser user = authService.Login(loginUser);
                        if (user != null)
                        {
                           
                            
                            UserService.SetLogin(user);

                        }
                       
                    }
                    
                }
                else if (loginRegister == 2)
                {
                    Console.Clear();
                    bool isRegistered = false;
                    while (!isRegistered) //will keep looping until user is registered
                    {
                        
                        LoginUser registerUser = consoleService.PromptForLogin();
                        isRegistered = authService.Register(registerUser);
                        if (isRegistered)
                        {
                            Console.Clear();
                            Console.WriteLine("");
                            Console.WriteLine();
                            Console.WriteLine();
                            Console.WriteLine();
                            Console.WriteLine();
                            Console.WriteLine();
                            Console.WriteLine();
                            Console.CursorLeft = 42;
                            Console.WriteLine("Registration successful. You can now log in.");
                            Console.WriteLine();
                            Console.WriteLine();
                            Console.WriteLine();
                            loginRegister = -1; //reset outer loop to allow choice for login
                            Console.CursorLeft = 47;
                            Console.WriteLine("Please press Enter to continue");
                            Console.CursorLeft = 47;
                            Console.ReadLine();
                            Console.Clear();
                        }
                    }
                }
                else
                {
                    Console.Clear();
                    Console.Beep(440, 200);
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    string warningHeader = FiggleFonts.Standard.Render("                                            Error");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(warningHeader);       // Shows there was an error at login 
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.CursorLeft = 47;
                    Console.WriteLine("Press Enter to continue");
                    Console.ReadLine();
                    Console.Clear();
                }
            }

            MenuSelection();
        }

        private static void MenuSelection()
        {
            string TenMoHeader = FiggleFonts.Speed.Render("                TENMO");
            int menuSelection = -1;
            while (menuSelection != 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine(TenMoHeader);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("");
                Console.CursorLeft = 37;
                Console.WriteLine("Welcome to TEnmo! Please make a selection: ");
                Console.WriteLine();
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.CursorLeft = 45;
                Console.WriteLine("1: View your current balance");
                Console.CursorLeft = 45;
                Console.WriteLine("2: View your past transfers");
                Console.CursorLeft = 45;
                Console.WriteLine("3: View your pending requests");
                Console.CursorLeft = 45;
                Console.WriteLine("4: Send TE bucks");
                Console.CursorLeft = 45;
                Console.WriteLine("5: Request TE bucks");
                Console.CursorLeft = 45;
                Console.WriteLine("6: Log in as different user");
                Console.CursorLeft = 45;
                Console.WriteLine("0: Exit");
                Console.CursorLeft = 45;
                Console.WriteLine("---------");
                Console.CursorLeft = 45;
                Console.Write("Please choose an option: ");

                if (!int.TryParse(Console.ReadLine(), out menuSelection))
                {
                    Console.WriteLine("Invalid input. Please enter only a number.");
                }
                else if (menuSelection == 1)            // Selection to see users available balance
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.CursorLeft = 47;
                    Console.WriteLine($"Your current balance is: ${acctservice.GetBalance()}");
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.CursorLeft = 50;
                    Console.WriteLine("Press Enter to continue");
                    Console.ReadLine();
                    Console.Clear();
                }
                else if (menuSelection == 2)        //Selection to list past transfers
                {
                    Console.Clear();
                    List<ApiTransfer> transfers = new List<ApiTransfer>();
                    transfers = acctservice.GetPastTransfer();
                    foreach(ApiTransfer apiTransfer in transfers)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.CursorLeft = 47;
                        Console.WriteLine("---------------------------------");
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.CursorLeft = 47;
                        Console.WriteLine("Transfer");
                        Console.CursorLeft = 47;
                        Console.WriteLine("Id          From/To        Amount");
                        Console.ForegroundColor = ConsoleColor.DarkBlue;
                        Console.CursorLeft = 47;
                        Console.WriteLine("---------------------------------");
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.CursorLeft = 47;
                        Console.WriteLine($"{apiTransfer.TransferId}       {apiTransfer.AccountFrom}|{apiTransfer.AccountTo}       ${apiTransfer.Amount}");
             
                    }
                    
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.CursorLeft = 42;
                    Console.WriteLine("Please enter transfer ID to view details (0 to cancel):");
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.CursorLeft = 65;
                    Console.ReadLine();
                    Console.Clear();
                    //if (!int.Parse(transferId))
                    //{
                    //    Console.WriteLine("Kinda works");
                    //}
                    ////else if (int.Parse(transferId)  != 0)
                    ////{
                    ////    Console.WriteLine("Kinda works");
                    ////}
                    //else
                    //{
                    //    Console.Clear();
                    //}

                }
                else if (menuSelection == 3)
                {

                }
                else if (menuSelection == 4)        // Selection to transfer Tenmo bucks
                {
                    
                    List<int> userAccounts = new List<int>();
                    userAccounts = acctservice.GetAccountIdsForTransfer();
                    int acctId = 0;
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    foreach(int account in userAccounts)
                    {
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.CursorLeft = 55;
                        Console.WriteLine($"| {account} |");
                    }
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.CursorLeft = 27;
                    Console.WriteLine("Please enter the id to whom you wish to send TE Bucks (Press 0 to cancel)");
                    Console.WriteLine();
                    Console.CursorLeft = 58;
                    if (!int.TryParse(Console.ReadLine(), out int userSelection))
                    {
                        Console.WriteLine("Invalid input. Only input a valid number");
                        userSelection = 0;
                        acctId = userSelection;
                    }
                    else
                    {
                        acctId = userSelection;
                    }
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.CursorLeft = 41;
                    Console.WriteLine("Please enter the amount you wish to transfer");
                    decimal amountToSend = 0;
                    Console.CursorLeft = 55;
                    if(!decimal.TryParse(Console.ReadLine(), out decimal userAmount))
                    {
                        Console.WriteLine("Invalid input. Please provide a valid dollar amount");
                        userAmount = 0;
                        amountToSend = userAmount;
                    }
                    else 
                    {
                        amountToSend = userAmount;
                    }
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    Console.CursorLeft = 46;
                    Console.WriteLine("Transfer in progress");
                    Console.WriteLine();
                    Console.CursorLeft = 45;
                    Console.WriteLine("Press Enter to continue");
                    Console.ReadLine();
                    Console.Clear();
                    acctservice.CreateTransfer(acctId, amountToSend);

                    
                    
                }
                else if (menuSelection == 5)
                {

                }
                else if (menuSelection == 6)
                {
                    Console.Clear();
                    Console.WriteLine("");
                    UserService.SetLogin(new ApiUser()); //wipe out previous login info
                    Run(); //return to entry point
                    
                }
                else
                {
                    Console.WriteLine("Goodbye!");
                    Environment.Exit(0);
                }
            }
        }
    }
}

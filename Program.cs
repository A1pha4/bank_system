using System;
using System.Collections.Generic;
using System.Text;

class Program
{
    class BankAccount
    {
        public string Name { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public int Pin { get; set; }
    }

    static List<BankAccount> accounts = new List<BankAccount>();

    static BankAccount currentUser = null;

    static void CreateAccount()
    {
        Console.Clear();
        ShowWelcomeMessage();
        Console.WriteLine("Account Creation");

        Console.WriteLine("Enter your name: ");
        string name = Console.ReadLine();

        string accountNumber = GenerateAccountNumber();
        int pin = GeneratePIN();

        BankAccount newAccount = new BankAccount
        {
            Name = name,
            AccountNumber = accountNumber,
            Balance = 0,
            Pin = pin
        };

        accounts.Add(newAccount);

        Console.WriteLine("Account created successfully.");
        Console.WriteLine($"Your account number is: {accountNumber}");
        Console.WriteLine($"Your PIN is: {pin:D4}");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    static string GenerateAccountNumber()
    {
        Random random = new Random();
        string accountNumber = string.Empty;
        for (int i = 0; i < 10; i++)
        {
            accountNumber += random.Next(0, 10);
        }
        return accountNumber;
    }

    static int GeneratePIN()
    {
        Random random = new Random();
        return random.Next(1000, 10000);
    }

    static void Login()
    {
        Console.Clear();
        ShowWelcomeMessage();
        Console.WriteLine("Account Login");

        Console.Write("Enter your account number: ");
        string accountNumber = Console.ReadLine();

        Console.Write("Enter your 4-digit PIN: ");
        int enteredPin = ReadHiddenPIN();

        currentUser = accounts.Find(account => account.AccountNumber == accountNumber && account.Pin == enteredPin);

        if (currentUser != null)
        {
            ShowTransactionMenu();
        }
        else
        {
            Console.WriteLine("Invalid account number or PIN. Press any key to continue...");
            Console.ReadKey();
        }
    }

    static int ReadHiddenPIN()
    {
         int pin = 0;
         string input = "";

         ConsoleKeyInfo key;
         do
         {
             key = Console.ReadKey(true);
             if (char.IsDigit(key.KeyChar) || key.Key == ConsoleKey.Backspace)
             {
                 if (key.Key == ConsoleKey.Backspace && input.Length > 0)
                 {
                     input = input.Substring(0, input.Length - 1);
                     Console.Write("\b \b");
                 }
                 else if (char.IsDigit(key.KeyChar))
                 {
                     input += key.KeyChar;
                     Console.Write("*"); 
                 }
             }
         } while (key.Key != ConsoleKey.Enter);

         if (int.TryParse(input, out pin))
         {
             return pin;
         }
         else
         {
             Console.WriteLine("Invalid PIN. Please enter a 4-digit numeric PIN.");
             return ReadHiddenPIN(); 
         }
    }

    static void ShowWelcomeMessage()
    {
        Console.WriteLine("\t\t\t\t\t****************************************************");
        Console.WriteLine("\t\t\t\t\t*                                                  *");
        Console.WriteLine("\t\t\t\t\t*             WELCOME TO THE ALPHA BANK            *");
        Console.WriteLine("\t\t\t\t\t*             MANAGEMENT SYSTEM                    *");
        Console.WriteLine("\t\t\t\t\t*                                                  *");
        Console.WriteLine("\t\t\t\t\t****************************************************");
        Console.WriteLine();
        Console.WriteLine();
    }

    static void ShowTransactionMenu()
    {
        while (true)
        {
            Console.Clear();
            ShowWelcomeMessage();
            Console.WriteLine();
            Console.WriteLine($"Welcome, {currentUser.Name}!");
            Console.WriteLine("Transaction Menu");
            Console.WriteLine("1. Deposit");
            Console.WriteLine("2. Withdraw");
            Console.WriteLine("3. Check Balance");
            Console.WriteLine("4. Transfer Money");
            Console.WriteLine("5. Delete Account");
            Console.WriteLine("6. Logout");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Deposit();
                    break;
                case "2":
                    Withdraw();
                    break;
                case "3":
                    CheckBalance();
                    break;
                case "4":
                    TransferMoney();
                    break;
                case "5":
                    DeleteAccount();
                    return;
                case "6":
                    currentUser = null;
                    return;
                default:
                    Console.WriteLine("Invalid choice. Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        
    
        }
    }

    static void Deposit()
    {
        Console.Clear();
        ShowWelcomeMessage();
        Console.WriteLine("Deposit");
        Console.Write("Enter the amount to deposit: ");
        decimal amount = Convert.ToDecimal(Console.ReadLine());

        currentUser.Balance += amount;

        PrintReceipt("Deposit", amount);
    }

    static void Withdraw()
    {
        Console.Clear();
        ShowWelcomeMessage();
        Console.WriteLine("Withdraw");
        Console.Write("Enter the amount to withdraw: ");
        decimal amount = Convert.ToDecimal(Console.ReadLine());

        if (currentUser.Balance >= amount)
        {
            currentUser.Balance -= amount;
            PrintReceipt("Withdrawal", amount);
        }
        else
        {
            Console.WriteLine("Insufficient funds. Press any key to continue...");
            Console.ReadKey();
        }
    }

    static void CheckBalance()
    {
        Console.Clear();
        ShowWelcomeMessage();
        Console.WriteLine("Check Balance");
        Console.WriteLine($"Your current balance is: {currentUser.Balance} KES");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    static void TransferMoney()
    {
        Console.Clear();
        ShowWelcomeMessage();
        Console.WriteLine("Transfer Money");
        Console.Write("Enter the recipient's account number: ");
        string recipientAccountNumber = Console.ReadLine();

        BankAccount recipient = accounts.Find(account => account.AccountNumber == recipientAccountNumber);

        if (recipient != null)
        {
            Console.Write("Enter the amount to transfer: ");
            decimal amount = Convert.ToDecimal(Console.ReadLine());

            if (currentUser.Balance >= amount)
            {
                currentUser.Balance -= amount;
                recipient.Balance += amount;

                PrintReceipt("Transfer", amount, recipient);
            }
            else
            {
                Console.WriteLine("Insufficient funds. Press any key to continue...");
                Console.ReadKey();
            }
        }
        else
        {
            Console.WriteLine("Recipient account not found. Press any key to continue...");
            Console.ReadKey();
        }
    }

    static void PrintReceipt(string transactionType, decimal amount, BankAccount recipient = null)
    {
        Console.Clear();
        ShowWelcomeMessage();
        Console.WriteLine();
        Console.WriteLine("Transaction Receipt");
        Console.WriteLine($"Name: {currentUser.Name}");
        Console.WriteLine($"Account Number: {currentUser.AccountNumber}");
        Console.WriteLine($"Transaction Type: {transactionType}");
        Console.WriteLine($"Amount: {amount} KES");
        Console.WriteLine($"Remaining Balance: {currentUser.Balance} KES");
        Console.WriteLine($"Date: {DateTime.Now}");

        if (recipient != null)
        {
            Console.WriteLine($"Recipient: {recipient.Name}");
            Console.WriteLine($"Recipient Account Number: {recipient.AccountNumber}");
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    static void DeleteAccount()
    {
        Console.Clear();
        Console.WriteLine("Account Deletion");

        Console.Write("Enter your account number for verification: ");
        string accountNumber = Console.ReadLine();

        BankAccount accountToDelete = accounts.Find(account => account.AccountNumber == accountNumber);

        if (accountToDelete != null && accountToDelete == currentUser)
        {
            Console.Write("Enter your 4-digit PIN for verification: ");
            int enteredPin = ReadHiddenPIN();

            if (enteredPin == currentUser.Pin)
            {
                accounts.Remove(accountToDelete);
                Console.WriteLine("Account deleted successfully. Press any key to continue...");
                Console.ReadKey();
                currentUser = null;
            }
            else
            {
                Console.WriteLine("Invalid PIN. Account deletion failed. Press any key to continue...");
                Console.ReadKey();
            }
        }
        else
        {
            Console.WriteLine("Invalid account number or you are not authorized to delete this account. Press any key to continue...");
            Console.ReadKey();
        }
    }

    static void Main()
    {
        while (true)
        {
            Console.Clear();
            ShowWelcomeMessage();   
            Console.WriteLine("Welcome to the Bank Management System!");
            Console.WriteLine("1. Create Account");
            Console.WriteLine("2. Login");
            Console.WriteLine("3. Exit");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateAccount();
                    break;
                case "2":
                    Login();
                    break;
                case "3":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice. Press any key to continue...");
                    Console.ReadKey();
                    break;
            }
        }
    }
}

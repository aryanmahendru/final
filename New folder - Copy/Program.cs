using System.Text.Json;


List<BankAccount> bankAccounts = new List<BankAccount>();
LoadBankAccounts();//loading bank accounts from json file

Console.WriteLine("Welcome to the Bank");
Console.WriteLine("Please select an option:");
Console.WriteLine("1. Create a Bank Account");
Console.WriteLine("2. ATM");
Console.WriteLine("3. Interbank Transfer");

string choice = Console.ReadLine();

if (choice == "1")
{
    CreateBankAccount();
}
else if (choice == "2")
{
    ATMSimulation();
}
else if (choice == "3")
{
    InterbankTransfer();
}
else
{
    Console.WriteLine("Invalid choice. please try again.");
}

SaveBankAccounts();

Console.WriteLine("Thank you");


 void CreateBankAccount()
{
    BankAccount newAccount = new BankAccount();

    Console.WriteLine("Enter your name:");
    string name = Console.ReadLine();
    while (string.IsNullOrEmpty(name))
    {
        Console.WriteLine("Name cannot be empty please enter your name:");
        name = Console.ReadLine();
    }
    newAccount.Name = name;

    Console.WriteLine("Enter your address:");
    string address = Console.ReadLine();
    while (string.IsNullOrEmpty(address))
    {
        Console.WriteLine("Address cannot be empty please enter your address:");
        address = Console.ReadLine();
    }
    newAccount.Address = address;

    decimal balance;
    Console.WriteLine("Enter the amount you want to deposit at the start:");
    while (!decimal.TryParse(Console.ReadLine(), out balance) || balance < 0)
    {
        Console.WriteLine("Invalid input please enter a valid amount:");
    }
    newAccount.Balance = balance;

    // Generating a random 10 digit account number
    Random random = new Random();
    string accountNumber;
    bool isDuplicate;
    do
    {
        accountNumber = random.Next(100000000, 999999999).ToString();
        isDuplicate = IsDuplicateAccountNumber(accountNumber);
    } while (isDuplicate);
    newAccount.AccountNumber = accountNumber;

    // Generating a random 12 digit debit card number
    string debitCardNumber;
    do
    {
        debitCardNumber = GenerateDebitCardNumber();
    } while (IsDuplicateDebitCardNumber(debitCardNumber));
    newAccount.DebitCardNumber = debitCardNumber;

    Console.WriteLine("Set a 4 digit PIN for your account:");
    string pin = Console.ReadLine();
    while (!IsNumeric(pin) || pin.Length != 4)
    {
        Console.WriteLine("Invalid pin please enter a 4-digit numeric PIN:");
        pin = Console.ReadLine();
    }
    newAccount.PIN = pin;

    Console.WriteLine("Enter your email address:");
    string emailAddress = Console.ReadLine();
    newAccount.EmailAddress = emailAddress;

    Console.WriteLine("Account created successfully");
    Console.WriteLine("account number: " + newAccount.AccountNumber);
    Console.WriteLine("debit card number: " + newAccount.DebitCardNumber);
    Console.WriteLine("PIN: " + newAccount.PIN);

    bankAccounts.Add(newAccount);
}

 bool IsNumeric(string input)
{
    return int.TryParse(input, out _);
}

 bool IsDuplicateAccountNumber(string accountNumber)
{
    foreach (BankAccount account in bankAccounts)
    {
        if (account.AccountNumber == accountNumber)
            return true;
    }
    return false;
}

 bool IsDuplicateDebitCardNumber(string debitCardNumber)
{
    foreach (BankAccount account in bankAccounts)
    {
        if (account.DebitCardNumber == debitCardNumber)
            return true;
    }
    return false;
}

 string GenerateDebitCardNumber()
{
    Random random = new Random();
    string debitCardNumber = string.Empty;
    for (int i = 0; i < 12; i++)
    {
        int digit = random.Next(0, 10);
        debitCardNumber += digit;
    }
    return debitCardNumber;
}

 void ATMSimulation()
{
    Console.WriteLine("Welcome to the ATM");

    Console.WriteLine("Enter your Debit Card Number:");
    string debitCardNumber = Console.ReadLine();

    Console.WriteLine("Enter your PIN:");
    string pin = Console.ReadLine();

    BankAccount account = AuthenticateUser(debitCardNumber, pin);

    if (account != null)
    {
        Console.WriteLine("Authentication successful");

        Console.WriteLine("Account Number: " + account.AccountNumber);
        Console.WriteLine("Name: " + account.Name);
        Console.WriteLine("Balance: " + account.Balance);

        Console.WriteLine("Please select an option:");
        Console.WriteLine("1 Withdraw Money");
        Console.WriteLine("2 Deposit Money");
        Console.WriteLine("3 View Transaction History");

        string option = Console.ReadLine();

        if (option == "1")
        {
            Console.WriteLine("Enter the amount you want to withdraw:");
            decimal amount;
            while (!decimal.TryParse(Console.ReadLine(), out amount) || amount < 0)
            {
                Console.WriteLine("Invalid input please enter a valid amount:");
            }

            if (amount <= account.Balance)
            {
                account.Balance -= amount;
                Console.WriteLine("Withdrawal successful!");
                Console.WriteLine("Remaining Balance: " + account.Balance);

                Transaction withdrawalTransaction = new Transaction
                {
                    Amount = amount,
                    Type = TransactionType.Withdrawal
                };
                account.TransactionHistory.Add(withdrawalTransaction);
            }
            else
            {
                Console.WriteLine("Insufficient balance withdrawal canceled.");
            }
        }
        else if (option == "2")
        {
            Console.WriteLine("Enter the amount you want to deposit:");
            decimal amount;
            while (!decimal.TryParse(Console.ReadLine(), out amount) || amount < 0)
            {
                Console.WriteLine("Invalid input please enter a valid amount:");
            }

            account.Balance += amount;
            Console.WriteLine("Deposit successful");
            Console.WriteLine("Updated Balance: " + account.Balance);

            Transaction depositTransaction = new Transaction
            {
                Amount = amount,
                Type = TransactionType.Deposit
            };
            account.TransactionHistory.Add(depositTransaction);
        }
        else if (option == "3")
        {
            Console.WriteLine("Transaction History:");
            foreach (Transaction transaction in account.TransactionHistory)
            {
                Console.WriteLine("Amount: " + transaction.Amount);
                Console.WriteLine("Type: " + transaction.Type);
                Console.WriteLine();
            }
        }
        else
        {
            Console.WriteLine("Invalid option please try again.");
        }
    }
    else
    {
        Console.WriteLine(" failed Invalid debit card number or PIN.");
    }
}

 void InterbankTransfer()
{
    Console.WriteLine("Enter your Debit Card Number:");
    string debitCardNumber = Console.ReadLine();

    Console.WriteLine("Enter your PIN :");
    string pin = Console.ReadLine();

    BankAccount account = AuthenticateUser(debitCardNumber, pin);

    if (account != null)
    {
        Console.WriteLine("Authentication successful");

        Console.WriteLine("Enter the recipients email address:");
        string recipientEmail = Console.ReadLine();

        Console.WriteLine("Enter the amount to transfer:");
        decimal amount;
        while (!decimal.TryParse(Console.ReadLine(), out amount) || amount < 0)
        {
            Console.WriteLine("Invalid input please enter a valid amount:");
        }

        Transaction transferTransaction = new Transaction
        {
            Amount = amount,
            Type = TransactionType.Transfer
        };
        account.TransactionHistory.Add(transferTransaction);

        Console.WriteLine("Transfer successful!");
        Console.WriteLine("Recipient: " + recipientEmail);
        Console.WriteLine("Amount: " + amount);
    }
    else
    {
        Console.WriteLine("Authentication failed invalid debit card number or PIN.");
    }
}

 BankAccount AuthenticateUser(string debitCardNumber, string pin)
{
    foreach (BankAccount account in bankAccounts)
    {
        if (account.DebitCardNumber == debitCardNumber && account.PIN == pin)
            return account;
    }
    return null;
}

 void LoadBankAccounts()
{
    try
    {
        string json = File.ReadAllText("bankaccounts.json");
        bankAccounts = JsonSerializer.Deserialize<List<BankAccount>>(json);
    }
    catch (Exception)
    {
        bankAccounts = new List<BankAccount>();
    }
}

 void SaveBankAccounts()
{
    string json = JsonSerializer.Serialize(bankAccounts);
    File.WriteAllText("bankaccounts.json", json);
}


class BankAccount
{
    public string Name { get; set; }
    public string Address { get; set; }
    public decimal Balance { get; set; }
    public string AccountNumber { get; set; }
    public string DebitCardNumber { get; set; }
    public string PIN { get; set; }
    public string EmailAddress { get; set; }
    public List<Transaction> TransactionHistory { get; set; }

    public BankAccount()
    {
        TransactionHistory = new List<Transaction>();
    }
}

enum TransactionType
{
    Withdrawal,
    Deposit,
    Transfer
}

class Transaction
{
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
}

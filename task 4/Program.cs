
using System;
using System.Collections.Generic;

// Account
public class Account
{
    public string Name { get; set; }
    public double Balance { get; set; }

    public Account(string name = "Unnamed Account", double balance = 0.0)
    {
        this.Name = name;
        this.Balance = balance;
    }

    public virtual bool Deposit(double amount)
    {
        if (amount < 0)
            return false;

        Balance += amount;
        return true;
    }

    public virtual bool Withdraw(double amount)
    {
        if (amount >= 0 && Balance - amount >= 0)
        {
            Balance -= amount;
            return true;
        }

        return false;
    }

    public override string ToString()
    {
        return $"{Name}: {Balance}";
    }
}

// SavingsAccount
public class SavingsAccount : Account
{
    public double InterestRate { get; set; }

    public SavingsAccount(string name = "Unnamed Savings Account",
        double balance = 0.0, double interestRate = 0.0)
        : base(name, balance)
    {
        InterestRate = interestRate;
    }

    public override bool Deposit(double amount)
    {
        if (amount < 0)
            return false;

        amount += amount * InterestRate / 100;
        return base.Deposit(amount);
    }
}

// CheckingAccount
public class CheckingAccount : Account
{
    public CheckingAccount(string name = "Unnamed Checking Account",
        double balance = 0.0)
        : base(name, balance)
    {
    }

    public override bool Withdraw(double amount)
    {
        double fee = 1.50;

        if (amount >= 0 && Balance >= amount + fee)
        {
            Balance -= amount + fee;
            return true;
        }

        return false;
    }
}

// TrustAccount
public class TrustAccount : SavingsAccount
{
    private int withdrawals = 0;

    public TrustAccount(string name = "Unnamed Trust Account",
        double balance = 0.0, double interestRate = 0.0)
        : base(name, balance, interestRate)
    {
    }

    public override bool Deposit(double amount)
    {
        if (amount < 0)
            return false;

        if (amount >= 5000)
            amount += 50;

        return base.Deposit(amount);
    }

    public override bool Withdraw(double amount)
    {
        if (withdrawals < 3 && amount >= 0 && amount <= Balance * 0.20)
        {
            if (base.Withdraw(amount))
            {
                withdrawals++;
                return true;
            }
        }

        return false;
    }
}

// AccountUtil
public static class AccountUtil
{
    public static void Deposit(List<Account> accounts, double amount)
    {
        Console.WriteLine("\n=== Depositing to Accounts ===");

        foreach (var acc in accounts)
        {
            if (acc.Deposit(amount))
                Console.WriteLine($"Deposited {amount} to {acc}");
            else
                Console.WriteLine($"Failed Deposit of {amount} to {acc}");
        }
    }

    public static void Withdraw(List<Account> accounts, double amount)
    {
        Console.WriteLine("\n=== Withdrawing from Accounts ===");

        foreach (var acc in accounts)
        {
            if (acc.Withdraw(amount))
                Console.WriteLine($"Withdrew {amount} from {acc}");
            else
                Console.WriteLine($"Failed Withdrawal of {amount} from {acc}");
        }
    }
}

// Program
class Program
{
    static void Main()
    {
        // Accounts
        var accounts = new List<Account>();
        accounts.Add(new Account());
        accounts.Add(new Account("Larry"));
        accounts.Add(new Account("Moe", 2000));
        accounts.Add(new Account("Curly", 5000));

        AccountUtil.Deposit(accounts, 1000);
        AccountUtil.Withdraw(accounts, 2000);

        // Savings
        var savAccounts = new List<Account>();
        savAccounts.Add(new SavingsAccount());
        savAccounts.Add(new SavingsAccount("Superman"));
        savAccounts.Add(new SavingsAccount("Batman", 2000));
        savAccounts.Add(new SavingsAccount("Wonderwoman", 5000, 5.0));

        AccountUtil.Deposit(savAccounts, 1000);
        AccountUtil.Withdraw(savAccounts, 2000);

        // Checking
        var checAccounts = new List<Account>();
        checAccounts.Add(new CheckingAccount());
        checAccounts.Add(new CheckingAccount("Larry2"));
        checAccounts.Add(new CheckingAccount("Moe2", 2000));
        checAccounts.Add(new CheckingAccount("Curly2", 5000));

        AccountUtil.Deposit(checAccounts, 1000);
        AccountUtil.Withdraw(checAccounts, 2000);
        AccountUtil.Withdraw(checAccounts, 2000);

        // Trust
        var trustAccounts = new List<Account>();
        trustAccounts.Add(new TrustAccount());
        trustAccounts.Add(new TrustAccount("Superman2"));
        trustAccounts.Add(new TrustAccount("Batman2", 2000));
        trustAccounts.Add(new TrustAccount("Wonderwoman2", 5000, 5.0));

        AccountUtil.Deposit(trustAccounts, 1000);
        AccountUtil.Deposit(trustAccounts, 6000);
        AccountUtil.Withdraw(trustAccounts, 2000);
        AccountUtil.Withdraw(trustAccounts, 3000);
        AccountUtil.Withdraw(trustAccounts, 500);

        Console.WriteLine();
    }
}


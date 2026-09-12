using System;
using System.Collections.Generic;

// =====================================================
// QUESTION 1: FINANCE MANAGEMENT SYSTEM
// =====================================================

// a. Transaction record
public record Transaction(
    int Id,
    DateTime Date,
    decimal Amount,
    string Category
);

// b. Transaction processor interface
public interface ITransactionProcessor
{
    void Process(Transaction transaction);
}

// c. Mobile Money Processor
public class MobileMoneyProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine(
            $"Mobile Money: Processing GHC{transaction.Amount:F2} for {transaction.Category}"
        );
    }
}

// c. Bank Transfer Processor
public class BankTransferProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine(
            $"Bank Transfer: Processing GHC{transaction.Amount:F2} for {transaction.Category}"
        );
    }
}

// c. Crypto Wallet Processor
public class CryptoWalletProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine(
            $"Crypto Wallet: Processing GHC{transaction.Amount:F2} for {transaction.Category}"
        );
    }
}

// d. General Account
public class Account
{
    public string AccountNumber { get; set; }

    public decimal Balance { get; protected set; }

    public Account(string accountNumber, decimal initialBalance)
    {
        AccountNumber = accountNumber;
        Balance = initialBalance;
    }

    public virtual void ApplyTransaction(Transaction transaction)
    {
        Balance -= transaction.Amount;
    }
}

// e. Specialized Savings Account
public sealed class SavingsAccount : Account
{
    public SavingsAccount(string accountNumber, decimal initialBalance)
        : base(accountNumber, initialBalance)
    {
    }

    public override void ApplyTransaction(Transaction transaction)
    {
        if (transaction.Amount > Balance)
        {
            Console.WriteLine("Insufficient funds");
        }
        else
        {
            Balance -= transaction.Amount;

            Console.WriteLine(
                $"Transaction applied. Updated balance: GHC{Balance:F2}"
            );
        }
    }
}

// f. FinanceApp
public class FinanceApp
{
    private List<Transaction> _transactions;

    public FinanceApp()
    {
        _transactions = new List<Transaction>();
    }

    public void Run()
    {
        // i. Create a SavingsAccount
        SavingsAccount account =
            new SavingsAccount("SA-001", 1000.00m);

        // ii. Create three Transaction records
        Transaction transaction1 = new Transaction(
            1,
            DateTime.Now,
            150.00m,
            "Groceries"
        );

        Transaction transaction2 = new Transaction(
            2,
            DateTime.Now,
            200.00m,
            "Utilities"
        );

        Transaction transaction3 = new Transaction(
            3,
            DateTime.Now,
            300.00m,
            "Entertainment"
        );

        // iii. Create processors
        MobileMoneyProcessor mobileMoney =
            new MobileMoneyProcessor();

        BankTransferProcessor bankTransfer =
            new BankTransferProcessor();

        CryptoWalletProcessor cryptoWallet =
            new CryptoWalletProcessor();

        Console.WriteLine("====================================");
        Console.WriteLine("       FINANCE MANAGEMENT SYSTEM");
        Console.WriteLine("====================================");
        Console.WriteLine();

        Console.WriteLine($"Account Number: {account.AccountNumber}");
        Console.WriteLine($"Initial Balance: GHC{account.Balance:F2}");
        Console.WriteLine();

        // Transaction 1
        mobileMoney.Process(transaction1);
        account.ApplyTransaction(transaction1);
        _transactions.Add(transaction1);

        Console.WriteLine();

        // Transaction 2
        bankTransfer.Process(transaction2);
        account.ApplyTransaction(transaction2);
        _transactions.Add(transaction2);

        Console.WriteLine();

        // Transaction 3
        cryptoWallet.Process(transaction3);
        account.ApplyTransaction(transaction3);
        _transactions.Add(transaction3);

        Console.WriteLine();
        Console.WriteLine("------------------------------------");
        Console.WriteLine($"Total Transactions: {_transactions.Count}");
        Console.WriteLine($"Final Balance: GHC{account.Balance:F2}");
        Console.WriteLine("------------------------------------");
    }
}

// Main application
class Program
{
    static void Main()
    {
        FinanceApp app = new FinanceApp();

        app.Run();
    }
}
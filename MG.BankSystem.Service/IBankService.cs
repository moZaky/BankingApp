namespace MG.BankSystem.Service
{
    public interface IBankService
    {
        double GetBalance(int accountId);
        void Withdraw(int accountId, double amount);
        void Deposit(int accountId, double amount);
    }
}

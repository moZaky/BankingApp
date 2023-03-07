namespace MG.BankSystem.Service
{
    public class BankService : IBankService
    {
        private readonly object balanceLock = new object();
        private readonly double[] _accounts;

        public BankService(double[] accounts)
        {
            _accounts = accounts;
        }
        public void Deposit(int accountId, double amount)
        {
            if (accountId < 0 || accountId >= _accounts.Length)
                throw new ArgumentException("Account not exist", accountId.ToString());
            if (amount <= 0)
                throw new ArgumentException("Invalid amount", amount.ToString());
            lock (balanceLock)
            {
               
                _accounts[accountId] += amount;
            }
        }

        public double GetBalance(int accountId)
        {
            if (accountId < 0 || accountId >= _accounts.Length)
                throw new ArgumentException("Account not exist", accountId.ToString());

            return _accounts[accountId];
        }

        public void Withdraw(int accountId, double amount)
        {
            if (accountId < 0 || accountId >= _accounts.Length)
                throw new ArgumentException("Account not exist", accountId.ToString());
            if (amount <= 0)
                throw new ArgumentException("Invalid amount", amount.ToString());
            if (amount > _accounts[accountId])
                throw new ArgumentException("Amount is greater than your balance", amount.ToString());

            lock (balanceLock)
            {
             
                _accounts[accountId] -= amount;
            }
        }
    }
}

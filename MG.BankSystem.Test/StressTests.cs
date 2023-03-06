using MG.BankSystem.Service;

namespace MG.BankSystem.Test
{
    public partial class FunctionalTests
    {
        public class StressTests
        {
            private const double InitialAmount = 10_000_000.0;
            private const int Timeout = 1500;
            private static readonly Random Rnd = new Random(10);
            [Theory(DisplayName = "Should perform all operations concurrently")]
            [InlineData(5, 1000)]
            [InlineData(5, 2000)]
            [InlineData(5, 5000)]
            [InlineData(5, 10000)]
            [InlineData(10, 1000)]
            [InlineData(10, 10000)]
            [InlineData(50, 500)]
            [InlineData(50, 1000)]
            [InlineData(100, 100)]
            [InlineData(100, 500)]
            public void ShouldPerformAllOperationsConcurrently(int numberOfAccounts, int operations)
            {
                var accounts = new double[numberOfAccounts];
                Array.Fill(accounts, InitialAmount);
                var bankingService = new BankService(accounts); // one service contains 100 account each account has 10M
                var actions = new List<Action>(); //1000 operations for 100 accounts
                var ids = Enumerable.Range(0, numberOfAccounts).ToArray(); // 0-99
                foreach (var id in ids) // 0-99
                {
                    for (var i = 0; i < operations; i++) // 0 - 499
                    {
                        actions.Add(() => { bankingService.Withdraw(id, 5); }); // 2500
                        actions.Add(() => { bankingService.Deposit(id, 4); });  // 2000
                        // 2500 - 2000 = 500
                    }
                }

                var shuffledActions = actions.OrderBy(x => Rnd.Next()).ToList(); // make the list as random.
                if (!Task.WaitAll(shuffledActions.Select(Task.Run).ToArray(), Timeout)) // run each action and all actions has 1.5 second to wait
                {
                    throw new TimeoutException();
                }
                
                foreach (var id in ids) // 0-99
                {
                    Assert.Equal(InitialAmount - operations, bankingService.GetBalance(id));
                }
            }
        }
    }
}
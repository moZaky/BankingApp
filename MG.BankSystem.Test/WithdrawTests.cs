using MG.BankSystem.Service;

namespace MG.BankSystem.Test
{
    public partial class FunctionalTests
    {
        public class WithdrawTests
        {
            private readonly IBankService _bankService;
            public WithdrawTests()
            {
                double[] accounts = { 1000.0, 1000.0, 1000.0 };
                _bankService = new BankService(accounts);
            }

            [Fact(DisplayName = "Should withdraw when amount is valid and account exists")]
            public void ShouldWithdrawSuccessfullyWhenAmountIsValidAndAccountExists()
            {
                // when
                _bankService.Withdraw(0, 100);
                _bankService.Withdraw(1, 200);
                _bankService.Withdraw(2, 300);

                // then
                Assert.Equal(900, _bankService.GetBalance(0));
                Assert.Equal(800, _bankService.GetBalance(1));
                Assert.Equal(700, _bankService.GetBalance(2));
            }

            [Theory(DisplayName = "Should throw ArgumentException when withdraw from non-existing account")]
            [InlineData(-1)]
            [InlineData(3)]
            public void ShouldThrowArgumentExceptionOnWithdrawalWhenAccountDoesNotExist(int outOfBoundId)

            {
                Assert.Throws<ArgumentException>(() => _bankService.Withdraw(outOfBoundId, 100));
            }

            [Theory(DisplayName = "Should throw ArgumentException when withdraw wrong amount of money")]
            [InlineData(0.0)]
            [InlineData(-100.0)]
            public void ShouldThrowArgumentExceptionOnWithdrawalWhenAmountIsInvalid(double invalidAmount)
            {
                Assert.Throws<ArgumentException>(() => _bankService.Withdraw(1, invalidAmount));
            }

            [Fact(DisplayName = "Should throw ArgumentException when withdraw too much money")]
            public void ShouldThrowArgumentExceptionOnWithdrawalWhenInsufficientMoney()
            {
                const double tooMuchMoney = 1500.0;
                Assert.Throws<ArgumentException>(() => _bankService.Withdraw(1, tooMuchMoney));
            }

        }
    }
}
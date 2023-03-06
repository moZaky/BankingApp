using MG.BankSystem.Service;

namespace MG.BankSystem.Test
{
    public partial class FunctionalTests
    {
        public class DepositTests
        {
            private readonly IBankService _bankingService;

            public DepositTests()
            {
                double[] accounts = { 1000, 1000, 1000 };
                _bankingService = new BankService(accounts);
            }

            [Fact(DisplayName = "Should deposit when amount is valid and account exists")]
            public void ShouldDepositSuccessfullyWhenAmountIsValidAndAccountExists()
            {
                // when
                _bankingService.Deposit(0, 100);
                _bankingService.Deposit(1, 200);
                _bankingService.Deposit(2, 300);

                // then
                Assert.Equal(1100, _bankingService.GetBalance(0));
                Assert.Equal(1200, _bankingService.GetBalance(1));
                Assert.Equal(1300, _bankingService.GetBalance(2));
            }

            [Theory(DisplayName = "Should throw ArgumentException when deposit to non-existing account")]
            [InlineData(-1)]
            [InlineData(3)]
            public void ShouldThrowArgumentExceptionOnDepositWhenAccountDoesNotExist(int outOfBoundId)
            {
                Assert.Throws<ArgumentException>(() => _bankingService.Deposit(outOfBoundId, 100));
            }

            [Theory(DisplayName = "Should throw ArgumentException when deposit wrong amount of money")]
            [InlineData(0.0)]
            [InlineData(-100.0)]
            public void ShouldThrowArgumentExceptionOnDepositWhenAmountIsInvalid(double invalidAmount)
            {
                Assert.Throws<ArgumentException>(() => _bankingService.Deposit(1, invalidAmount));
            }

        }
    }
}
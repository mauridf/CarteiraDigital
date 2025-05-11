using CarteiraDigital.Application.Common.Interfaces;
using CarteiraDigital.Application.Common.Services;
using CarteiraDigital.Domain.Entities;
using CarteiraDigital.Domain.Repositories;
using Moq;

namespace CarteiraDigital.UnitTests.ApplicationTests
{
    public class WalletServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock = new();
        private readonly Mock<IWalletRepository> _walletRepositoryMock = new();
        private readonly Mock<ITransactionRepository> _transactionRepositoryMock = new();
        private readonly IWalletService _walletService;

        public WalletServiceTests()
        {
            _walletService = new WalletService(
                _userRepositoryMock.Object,
                _walletRepositoryMock.Object,
                _transactionRepositoryMock.Object);
        }

        [Fact]
        public async Task GetBalanceAsync_WithValidUserId_ReturnsBalance()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User("Test", "User", "test@example.com", "hashed_password");
            var wallet = new Wallet(user) { Balance = 1000 };

            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _walletRepositoryMock.Setup(x => x.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(wallet);

            // Act
            var balance = await _walletService.GetBalanceAsync(userId);

            // Assert
            Assert.Equal(1000, balance);
        }

        [Fact]
        public async Task DepositAsync_WithValidAmount_UpdatesBalance()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User("Test", "User", "test@example.com", "hashed_password");
            var wallet = new Wallet(user);

            _userRepositoryMock.Setup(x => x.GetByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);
            _walletRepositoryMock.Setup(x => x.GetByUserIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(wallet);

            // Act
            await _walletService.DepositAsync(userId, 500);

            // Assert
            Assert.Equal(500, wallet.Balance);
            _walletRepositoryMock.Verify(x => x.UpdateAsync(wallet, It.IsAny<CancellationToken>()), Times.Once);
            _transactionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task TransferAsync_WithValidData_TransfersAmount()
        {
            // Arrange
            var fromUserId = Guid.NewGuid();
            var toUserId = Guid.NewGuid();

            var fromUser = new User("From", "User", "from@example.com", "hashed_password");
            var toUser = new User("To", "User", "to@example.com", "hashed_password");

            var fromWallet = new Wallet(fromUser) { Balance = 1000 };
            var toWallet = new Wallet(toUser);

            _userRepositoryMock.Setup(x => x.GetByIdAsync(fromUserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(fromUser);
            _userRepositoryMock.Setup(x => x.GetByIdAsync(toUserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(toUser);

            _walletRepositoryMock.Setup(x => x.GetByUserIdAsync(fromUserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(fromWallet);
            _walletRepositoryMock.Setup(x => x.GetByUserIdAsync(toUserId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(toWallet);

            // Act
            await _walletService.TransferAsync(fromUserId, toUserId, 500);

            // Assert
            Assert.Equal(500, fromWallet.Balance);
            Assert.Equal(500, toWallet.Balance);
            _walletRepositoryMock.Verify(x => x.UpdateAsync(fromWallet, It.IsAny<CancellationToken>()), Times.Once);
            _walletRepositoryMock.Verify(x => x.UpdateAsync(toWallet, It.IsAny<CancellationToken>()), Times.Once);
            _transactionRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Transaction>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        }
    }
}

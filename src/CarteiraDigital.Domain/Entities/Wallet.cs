using System.Transactions;

namespace CarteiraDigital.Domain.Entities
{
    public class Wallet
    {
        public Guid Id { get; private set; }
        public User User { get; private set; }
        public decimal Balance { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime? UpdatedOn { get; private set; }
        public ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();

        public Wallet(User user)
        {
            Id = Guid.NewGuid();
            User = user;
            Balance = 0;
            CreatedOn = DateTime.UtcNow;
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.");

            Balance += amount;
            UpdatedOn = DateTime.UtcNow;
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero.");

            if (Balance < amount)
                throw new InvalidOperationException("Insufficient funds.");

            Balance -= amount;
            UpdatedOn = DateTime.UtcNow;
        }
    }
}

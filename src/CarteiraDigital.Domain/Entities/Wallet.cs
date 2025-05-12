namespace CarteiraDigital.Domain.Entities
{
    public class Wallet
    {
        public Guid Id { get; private set; }
        public User User { get; private set; }
        public Guid UserId { get; private set; }
        public decimal Balance { get; set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime? UpdatedOn { get; private set; }
        public ICollection<Transaction> Transactions { get; private set; } = new List<Transaction>();

        // Construtor privado para o EF Core
        private Wallet() { }

        // Método factory para criação
        public static Wallet Create(Guid userId)
        {
            return new Wallet
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Balance = 0,
                CreatedOn = DateTime.UtcNow
            };
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
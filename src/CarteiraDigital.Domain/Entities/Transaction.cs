namespace CarteiraDigital.Domain.Entities
{
    public enum TransactionType
    {
        Deposit,
        Withdrawal,
        Transfer
    }

    public enum TransactionStatus
    {
        Pending,
        Completed,
        Failed
    }

    public class Transaction
    {
        public Guid Id { get; private set; }
        public Wallet Wallet { get; private set; }
        public Guid WalletId { get; private set; }
        public decimal Amount { get; private set; }
        public TransactionType Type { get; private set; }
        public TransactionStatus Status { get; private set; }
        public string? Description { get; private set; }
        public DateTime CreatedOn { get; private set; }
        public DateTime? CompletedOn { get; private set; }
        public Wallet? RelatedWallet { get; private set; }
        public Guid RelatedWalletId { get; private set; }

        public static Transaction Create(
        Guid walletId,
        decimal amount,
        TransactionType type,
        string? description = null,
        Guid? relatedWalletId = null)
        {
            return new Transaction
            {
                Id = Guid.NewGuid(),
                WalletId = walletId,
                Amount = amount,
                Type = type,
                Description = description,
                RelatedWalletId = relatedWalletId ?? Guid.Empty,
                Status = TransactionStatus.Pending,
                CreatedOn = DateTime.UtcNow
            };
        }

        public void Complete()
        {
            Status = TransactionStatus.Completed;
            CompletedOn = DateTime.UtcNow;
        }

        public void Fail()
        {
            Status = TransactionStatus.Failed;
            CompletedOn = DateTime.UtcNow;
        }
    }
}

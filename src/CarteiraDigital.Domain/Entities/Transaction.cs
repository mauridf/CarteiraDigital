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

        public Transaction(
            Wallet wallet,
            decimal amount,
            TransactionType type,
            string? description = null,
            Wallet? relatedWallet = null)
        {
            Id = Guid.NewGuid();
            Wallet = wallet;
            Amount = amount;
            Type = type;
            Status = TransactionStatus.Pending;
            Description = description;
            CreatedOn = DateTime.UtcNow;
            RelatedWallet = relatedWallet;
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

namespace CarteiraDigital.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string FullName => $"{FirstName} {LastName}";
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public DateTime? UpdatedOn { get; private set; }
    public Wallet Wallet { get; private set; }

    public User(string firstName, string lastName, string email, string passwordHash)
    {
        Id = Guid.NewGuid();
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        PasswordHash = passwordHash;
        CreatedOn = DateTime.UtcNow;
        Wallet = new Wallet(this);
    }

    // Métodos para atualização
    public void UpdateName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
        UpdatedOn = DateTime.UtcNow;
    }

    public void UpdatePassword(string passwordHash)
    {
        PasswordHash = passwordHash;
        UpdatedOn = DateTime.UtcNow;
    }
}
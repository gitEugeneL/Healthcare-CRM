using Domain.Common;

namespace Domain.Offices;

public sealed class Office : BaseEntity
{
    private Office() { }

    public string Name { get; private set; } = null!;
    public int Number { get; private init; }  
    public bool IsAvailable { get; private set; }


    public static Office Create(string name, int number)
    {
        var office = new Office
        {
            Name = name.Trim().ToUpperInvariant(),
            Number = number,
            IsAvailable = true
        };
        return office;
    }

    public void ChangeAvailability()
    {
        IsAvailable = !IsAvailable;
    }

    public void ChangeName(string name)
    {
        var normalized = name.Trim().ToUpperInvariant();
        
        if (Name == normalized)
            return;

        Name = normalized;
    }
}

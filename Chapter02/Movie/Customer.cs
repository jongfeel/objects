public class Customer
{
    public string Name { get; set; } = "Anonymous";

    public override string ToString() => Name;
}
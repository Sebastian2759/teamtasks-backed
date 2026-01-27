namespace Domain.Base;

public class DomainValidation
{
    public Dictionary<string, string> Fails { get; private set; }

    public bool IsValid { get => Fails.Count == 0; }

    public DomainValidation()
    {
        Fails = new Dictionary<string, string>();
    }

    public void AddFailed(string key, string error)
    {
        Fails.Add(key, error);
    }
}
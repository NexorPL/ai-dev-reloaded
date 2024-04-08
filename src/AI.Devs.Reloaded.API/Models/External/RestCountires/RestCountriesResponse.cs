public class RestCountriesResponse
{
    public Name? Name { get; set; }
    public int Population { get; set; }
}

public class Name
{
    public string? Common { get; set; }
    public string? Official { get; set; }
}

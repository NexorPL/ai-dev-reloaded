namespace AI.Devs.Reloaded.API.Models.External.NBP;

public class NbpResponse
{
    public required string Table { get; set; }
    public required string Currency { get; set; }
    public required string Code { get; set; }
    public required List<Rate> Rates { get; set; }
}

public class Rate
{
    public required string No { get; set; }
    public required DateTime EffectiveDate { get; set; }
    public required double Mid { get; set; }
}

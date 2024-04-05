namespace AI.Devs.Reloaded.API.Configurations;

public class QdrantOptions
{
    public const string Qdrant = "QdrantSettings";

    public required string GRpcUrl { get; set; }
    public required string CertificateThumbprint { get; set; }
}

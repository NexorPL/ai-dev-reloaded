namespace AI.Devs.Reloaded.API.Contracts.OpenAi.Moderation;

public sealed record CategoryScores(
    double Sexual,
    double Hate,
    double Harassment,
    double SelfHarm,
    double SexualMinors,
    double HateThreatening,
    double ViolenceGraphic,
    double SelfHarmIntent,
    double SelfHarmInstructions,
    double HarassmentThreatening,
    double Violence
)
{ };

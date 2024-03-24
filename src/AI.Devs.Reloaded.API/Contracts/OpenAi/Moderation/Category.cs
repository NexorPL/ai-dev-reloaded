namespace AI.Devs.Reloaded.API.Contracts.OpenAi.Moderation;

public sealed record Category(
    bool Sexual,
    bool Hate,
    bool Harassment,
    bool SelfHarm,
    bool SexualMinors,
    bool HateThreatening,
    bool ViolenceGraphic,
    bool SelfHarmIntent,
    bool SelfHarmInstructions,
    bool HarassmentThreatening,
    bool Violence
)
{ };

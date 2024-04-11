using System.Text.Json;
using AI.Devs.OwnApi.Utils.Consts;

namespace AI.Devs.OwnApi.Contracts.OpenAi.Completions;

public sealed record Response(
    string id,
    string Object,
    long created,
    string model,
    string systemFingerprint,
    List<Choice> choices,
    Usage usage
)
{
    public string AssistanceFirstMessage { get => choices.Single(x => x.message.role == OpenAiApi.Roles.Assistant).message.content; }

    public T DeserializeToModel<T>()
        where T : class
    {
        var model = JsonSerializer.Deserialize<T>(AssistanceFirstMessage);
        return model!;
    }
};

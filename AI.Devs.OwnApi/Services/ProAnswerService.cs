using System.Collections.Concurrent;
using AI.Devs.OwnApi.HttpClients.Abstractions;
using AI.Devs.OwnApi.Models.AiDevs;

namespace AI.Devs.OwnApi.Services;

public interface IProAnswerService
{
    Task<IResult> ProAnswer(Models.AiDevs.Request request, CancellationToken ct);
}

public class ProAnswerService(IOpenAiClient client) : IProAnswerService
{
    private static readonly ConcurrentDictionary<Guid, List<Contracts.OpenAi.Completions.Message>> _contextConversation = new();
    private readonly IOpenAiClient _client = client;

    public async Task<IResult> ProAnswer(Request request, CancellationToken ct)
    {
        if (request is null) throw new ArgumentNullException(nameof(request));

        //Consider using some lock mechanizm to block/wait multiply invokaction for one uuid
        if (!_contextConversation.TryGetValue(request.UUID, out List<Contracts.OpenAi.Completions.Message>? value))
        {
            var systemPrompt = "Remember information about user. Every provided information about user say ONLY \"Rozumiem\".Answer strictly, concisely about user information.";

            var messages = new List<Contracts.OpenAi.Completions.Message>()
            {
                Contracts.OpenAi.Completions.Message.CreateSystemMessage(systemPrompt),
                Contracts.OpenAi.Completions.Message.CreateUserMessage(request.question!)
            };

            _contextConversation.TryAdd(request.UUID, messages);
        }
        else
        {
            value.Add(Contracts.OpenAi.Completions.Message.CreateUserMessage(request.question!));
        }
        
        var aiResponse = await _client.CompletionsAsync(_contextConversation[request.UUID], ct);

        _contextConversation[request.UUID].Add(Contracts.OpenAi.Completions.Message.CreateAssistantMessage(aiResponse.AssistanceFirstMessage));

        var response = new Response(aiResponse.AssistanceFirstMessage);

        return Results.Ok(response);
    }
}

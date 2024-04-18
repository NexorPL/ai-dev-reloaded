using AI.Devs.OwnApi.HttpClients.Abstractions;
using AI.Devs.OwnApi.Models;

namespace AI.Devs.OwnApi.Services;

public interface IGoogleAnswerService 
{
    Task<IResult> Answer(Models.AiDevs.Request request, CancellationToken ct);
}

public class GoogleAnswerService(IOpenAiClient client, ISerpClient serpClient) : IGoogleAnswerService
{
    private readonly IOpenAiClient _client = client;
    private readonly ISerpClient _serpClient = serpClient;

    public async Task<IResult> Answer(Models.AiDevs.Request request, CancellationToken ct)
    {
        var tools = PrepareTools();
        var aiResponse = await _client.CompletionsAsync(SYSTEM_PROMPT, request.question!, tools, ct);
        string answer;

        if (aiResponse.ExistFunctionCalling)
        {
            var serpResponse = await _serpClient.SearchAsync(request.question!, ct);
            answer = serpResponse.organic_results.First().Link!;
        }
        else
        {
            answer = aiResponse.DeserializeToModel<GoogleAiModelResponse>().result;
        }

        var response = new Models.AiDevs.Response(answer!);
        return Results.Ok(response);
    }

    private static List<Contracts.OpenAi.Completions.Tool> PrepareTools()
    {
        var parameters = new Contracts.OpenAi.Completions.SearchParamFuncton() { Type = "string", Description = "search query to te engine" };
        
        var tools = new List<Contracts.OpenAi.Completions.Tool>
        {
            new("function", new("Search information", "searchSerp", parameters))
        };

        return tools;
    }

    private const string SYSTEM_PROMPT = "You are an assistance. You are only answering to the provided question. Answer strictly, concisely and ONLY in JSON {\"type\": \"response|search\", \"result\": \"answer|dont know\"}\n\nExample###\n- {\"type\": \"response\",  \"result\": \"https://wp.pl\"}\n- {\"type\": \"search\", \"result\": \"dont know\"}\n###";
}

using AI.Devs.Reloaded.API.Configurations;
using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.Models.External.NBP;
using AI.Devs.Reloaded.API.Tasks.Abstractions;
using AI.Devs.Reloaded.API.Tasks.Models;
using AI.Devs.Reloaded.API.Utils.Consts;
using Microsoft.Extensions.Options;

namespace AI.Devs.Reloaded.API.Tasks;

public class TaskKnowledge(
    IOpenAiClient openAiClient, 
    ITaskClient client, 
    ICustomApiClient customApiClient, 
    IOptions<KnowledgeApiOptions> knowledgeOptions
    ) : TaskSolver<string>(openAiClient, client), ITaskKnowledge
{
    private readonly ICustomApiClient _customApiClient = customApiClient;
    private readonly IOptions<KnowledgeApiOptions> _knowledgeOptions = knowledgeOptions;

    public override AiDevsDefs.TaskEndpoints GetEndpoint() => AiDevsDefs.TaskEndpoints.Knowledge;

    public override async Task<string> PrepareAnswer(TaskResponse taskResponse, CancellationToken cancellationToken)
    {
        var messages = PrepareData(taskResponse);
        var aiReponse = await _openAiClient.CompletionsAsync(messages, cancellationToken);
        var knowledgeResponse = aiReponse.DeserializeToModel<KnowledgeAiResponse>();
        
        var apiKind = knowledgeResponse.ApiKind;

        var resultAnswer = apiKind switch
        {
            AllowedKnowledgeAiApiKind.Currency => await InvokeNbp(knowledgeResponse.Answer, cancellationToken),
            AllowedKnowledgeAiApiKind.Population => await InvokeRestCountries(knowledgeResponse.Answer, cancellationToken),
            AllowedKnowledgeAiApiKind.GeneralKnwoledge => knowledgeResponse.Answer,
            _ => throw new NotSupportedException()
        };

        return resultAnswer;
    }
    
    private static List<Contracts.OpenAi.Completions.Message> PrepareData(TaskResponse response)
    {
        var systemMessage = @$"
Return only JSON {{""ApiKind"": ""{AllowedKnowledgeAiApiKind.Currency}|{AllowedKnowledgeAiApiKind.Population}|{AllowedKnowledgeAiApiKind.GeneralKnwoledge}"", ""Answer"": ""USD|<country_that_user_ask>|<response to message - shortly and percise>""}}

Examples ###
Jak nazywa się stolica Polski?
{{""ApiKind"": ""general_knwoledge"", ""Answer"": ""Warszawa""}}

Jaki jest kurs dolara amerykańskiego?
{{""ApiKind"": ""currency"", ""Answer"": ""USD""}}

Jaka jest populacja Polski?
{{""ApiKind"": ""population"", ""Answer"": ""Poland""}}
###
";

        var systemPrompt = Contracts.OpenAi.Completions.Message.CreateSystemMessage(systemMessage);
        var userPrompt = Contracts.OpenAi.Completions.Message.CreateUserMessage(response.question!);
        var messages = new List<Contracts.OpenAi.Completions.Message>() { systemPrompt, userPrompt };

        return messages;
    }

    private async Task<string> InvokeNbp(string currency, CancellationToken cancellationToken)
    {
        var url = _knowledgeOptions.Value.NbpUrl.Replace("{Currency}", currency);
        var response = await _customApiClient.GetAsync<NbpResponse>(url, cancellationToken);
        return response.Rates.First().Mid.ToString();
    }

    private async Task<string> InvokeRestCountries(string question, CancellationToken cancellationToken)
    {
        var response = await _customApiClient.GetAsync<List<RestCountriesResponse>>(_knowledgeOptions.Value.RestCountriesUrl, cancellationToken);
        var country = response.Single(x => x.Name!.Common!.Equals(question, StringComparison.OrdinalIgnoreCase));
        return country.Population.ToString();
    }
}

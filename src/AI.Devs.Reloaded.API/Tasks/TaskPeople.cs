using System.Text.Json;
using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.Extensions;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.Models;
using AI.Devs.Reloaded.API.Models.OpenAi;
using AI.Devs.Reloaded.API.Tasks.Abstractions;
using AI.Devs.Reloaded.API.Utils.Consts;

namespace AI.Devs.Reloaded.API.Tasks;

public class TaskPeople(IOpenAiClient openAiClient, ITaskClient client) : TaskSolver<string>(openAiClient, client), ITaskPeople
{
    public override AiDevsDefs.TaskEndpoints GetEndpoint() => AiDevsDefs.TaskEndpoints.People;

    public override async Task<string> PrepareAnswer(TaskResponse taskResponse, CancellationToken cancellationToken)
    {
        using var stream = await _client.GetFileAsync(taskResponse.data!, cancellationToken);
        using var reader = new StreamReader(stream);
        var peopleJson = await reader.ReadToEndAsync(cancellationToken);
        var peopleList = JsonSerializer.Deserialize<List<PeopleModel>>(peopleJson)!;
        var messages = PrepareData(taskResponse);

        var peopleOpenAiResponse = await _openAiClient.CompletionsAsync(messages, cancellationToken);
        var peopleResponse = peopleOpenAiResponse.DeserializeToModel<PeopleResponse>();
        var propertyName = peopleResponse.Property;
        var person = peopleList.FindByFirstAndLastname(peopleResponse);
        string finalAnswer = "";

        if (peopleResponse.Property.Equals("AboutMe", StringComparison.OrdinalIgnoreCase))
        {
            var systemPrompt = PrepareSystemPromptAboutMe(person);
            var aboutMeResponse = await _openAiClient.CompletionsAsync(systemPrompt, taskResponse.question!, cancellationToken);
            finalAnswer = aboutMeResponse.AssistanceFirstMessage;
        }
        else
        {
            var property = person!.GetType().GetProperty(propertyName);
            finalAnswer = property!.GetValue(person!)!.ToString()!;
        }

        return finalAnswer;
    }

    public static List<Contracts.OpenAi.Completions.Message> PrepareData(TaskResponse response)
    {
        var systemMessage = @"
Convert provided input into JSON {""Firstname"": ""Jan"", ""Lastname"": ""Nowak"", ""Property"": ""wiek (Age)|informacje o osobie (AboutMe)|ulubiona postać z kapitana bomby (FavoriteCharacterFromKapitanBomba)|ulubiony serial (FavoriteSeries)| ulubiony film (FavoriteMovie)|ulubiony kolor (FavoriteColour)""

Example###
jaki kolor się podoba Mariuszowi Kaczorowi?
{""Firstname"": ""Mariusz"", ""Lastname"": ""Kaczor"", ""Property"": ""FavoriteColour""}
Gdzie mieszka Krysia Ludek?
{""Firstname"": ""Krystyna"", ""Lastname"": ""Ludek"", ""Property"": ""AboutMe""}
""powiedz mi, jaki jest ulubiony serial Katarzyny Truskawek?
{""Firstname"": ""Katarzyna"": ""Truskawek"": ""Property"": ""FavoriteSeries""}
jaki film podoba sie Mariuszowi Kaczorowi?
{""Firstname"": ""Mariusz"", ""Lastname"": ""Kaczor"", ""Property"": ""FavoriteMovie""}
Jaka jest ulubiona postać z kreskówki kapitana bomby Krystyna Krowy?
{""Firstname"": ""Krystyna"", ""Lastname"": ""Krowa"", ""Property"": ""FavoriteCharacterFromKapitanBomba""}
###";

        var systemPrompt = Contracts.OpenAi.Completions.Message.CreateSystemMessage(systemMessage);
        var userPrompt = Contracts.OpenAi.Completions.Message.CreateUserMessage(response.question!);
        var messages = new List<Contracts.OpenAi.Completions.Message>() { systemPrompt, userPrompt };

        return messages;
    }

    public static string PrepareSystemPromptAboutMe(PeopleModel person)
    {
        var prompt = @$"
Return ONLY STRICT information about person provided about the user in one word

Facts ###
{person.FullName} - {person.AboutMe}
###";

        return prompt;
    }
}

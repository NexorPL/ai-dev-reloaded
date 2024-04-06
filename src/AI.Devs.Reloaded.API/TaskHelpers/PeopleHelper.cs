using System.Text;
using System.Text.Json;
using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.Models;
using AI.Devs.Reloaded.API.Models.OpenAi;
using AI.Devs.Reloaded.API.Utils.Consts;

namespace AI.Devs.Reloaded.API.TaskHelpers;

public static class PeopleHelper
{
    public static async Task<List<PeopleModel>> ConvertToListPeopleModel(Stream stream, CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(stream);
        var peopleJson = await reader.ReadToEndAsync(cancellationToken);

        return JsonSerializer.Deserialize<List<PeopleModel>>(peopleJson)!;
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

        var systemPrompt = new Contracts.OpenAi.Completions.Message(OpenAiApi.Roles.System, systemMessage);
        var userPrompt = new Contracts.OpenAi.Completions.Message(OpenAiApi.Roles.User, response.question!);

        var messages = new List<Contracts.OpenAi.Completions.Message>() { systemPrompt, userPrompt };
        return messages;
    }

    public static PeopleResponse ParseResponse(Contracts.OpenAi.Completions.Response response)
    {
        var contentJson = response.choices.Single(x => x.message.role == OpenAiApi.Roles.Assistant).message.content;
        var content = JsonSerializer.Deserialize<PeopleResponse>(contentJson);

        return content!;
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

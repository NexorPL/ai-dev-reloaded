using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.Extensions;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.TaskHelpers;
using AI.Devs.Reloaded.API.Tasks.Abstractions;
using AI.Devs.Reloaded.API.Utils.Consts;

namespace AI.Devs.Reloaded.API.Tasks;

public class TaskPeople(IOpenAiClient openAiClient, ITaskClient client) : TaskSolver<string>(openAiClient, client), ITaskPeople
{
    public override AiDevsDefs.TaskEndpoints GetEndpoint() => AiDevsDefs.TaskEndpoints.People;

    public override async Task<string> PrepareAnswer(TaskResponse taskResponse, CancellationToken cancellationToken)
    {
        using var stream = await _client.GetFileAsync(taskResponse.data!, cancellationToken);
        var peopleList = await PeopleHelper.ConvertToListPeopleModel(stream, cancellationToken);
        var messages = PeopleHelper.PrepareData(taskResponse);

        var peopleOpenAiResponse = await _openAiClient.CompletionsAsync(messages, cancellationToken);
        var peopleResponse = PeopleHelper.ParseResponse(peopleOpenAiResponse);

        var propertyName = peopleResponse.Property;
        var person = peopleList.FindByFirstAndLastname(peopleResponse);
        string finalAnswer = "";

        if (peopleResponse.Property.Equals("AboutMe", StringComparison.OrdinalIgnoreCase))
        {
            var systemPrompt = PeopleHelper.PrepareSystemPromptAboutMe(person);
            var aboutMeResponse = await _openAiClient.CompletionsAsync(systemPrompt, taskResponse.question!, cancellationToken);
            finalAnswer = aboutMeResponse.choices.Single(x => x.message.role == OpenAiApi.Roles.Assistant).message.content;
        }
        else
        {
            var property = person!.GetType().GetProperty(propertyName);
            finalAnswer = property!.GetValue(person!)!.ToString()!;
        }

        return finalAnswer;
    }
}

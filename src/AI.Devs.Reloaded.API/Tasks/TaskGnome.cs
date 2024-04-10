using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.Contracts.OpenAi.Completions;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.Tasks.Abstractions;
using AI.Devs.Reloaded.API.Utils.Consts;

namespace AI.Devs.Reloaded.API.Tasks;

public class TaskGnome(IOpenAiClient openAiClient, ITaskClient client) : TaskSolver<string>(openAiClient, client), ITaskGnome
{
    public override AiDevsDefs.TaskEndpoints GetEndpoint() => AiDevsDefs.TaskEndpoints.Gnome;

    public override async Task<string> PrepareAnswer(TaskResponse taskResponse, CancellationToken cancellationToken)
    {
        var userContent = VisionContent.CreateTextContent("Return ONLY the colour of the hat in POLISH. If there are no hats or some error return ONLY error");
        var imageContent = VisionContent.CreateImageUrlContent(taskResponse.url!, "low");
        var userPrompt = MessageVision.CreateUserMessage(new() { userContent, imageContent });
        
        var request = new List<MessageVision>() { userPrompt };
        var aiResponse = await _openAiClient.CompletionsVisionAsync(request, cancellationToken);
        return aiResponse.AssistanceFirstMessage;
    }
}

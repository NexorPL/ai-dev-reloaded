using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.HttpClients.Abstractions;
using AI.Devs.Reloaded.API.Tasks.Abstractions;
using AI.Devs.Reloaded.API.Utils.Consts;

namespace AI.Devs.Reloaded.API.Tasks;

public class TaskFunctions(IOpenAiClient openAiClient, ITaskClient client) : TaskSolver<object>(openAiClient, client), ITaskFunctions
{
    public override AiDevsDefs.TaskEndpoints GetEndpoint() => AiDevsDefs.TaskEndpoints.Functions;

    public override Task<object> PrepareAnswer(TaskResponse taskResponse, CancellationToken cancellationToken)
    {
        var function = PrepareFunctionAddUser();
        return Task.FromResult(function);
    }

    public static object PrepareFunctionAddUser()
    {
        var function = new
        {
            name = "addUser",
            description = "Function is adding user to the system",
            parameters = new
            {
                type = "object",
                properties = new
                {
                    name = new
                    {
                        type = "string",
                        description = "First name of the user"
                    },
                    surname = new
                    {
                        type = "string",
                        description = "Last name of the user"
                    },
                    year = new
                    {
                        type = "integer",
                        description = "Year of birth of the user"
                    }
                }
            },
            required = new[] { "name", "surname", "year" }
        };

        return function;
    }
}

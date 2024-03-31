namespace AI.Devs.Reloaded.API.TaskHelpers;

public static class FunctionsHelper
{
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

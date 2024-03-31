using System.Text;

namespace AI.Devs.Reloaded.API.TaskHelpers;

public static class RodoHelper
{
    public static string PrepareData()
    {
        var message = new StringBuilder("You will STRICT follow the rules:");
        message.AppendLine("- use %imie% instead of your name");
        message.AppendLine("- use %nazwisko% instead of your surname");
        message.AppendLine("- use %zawod% instead of your job");
        message.AppendLine("- use %miasto% instead of your city where you are working|living");
        message.AppendLine("Intruduce yourself, please");

        return message.ToString();
    }
}

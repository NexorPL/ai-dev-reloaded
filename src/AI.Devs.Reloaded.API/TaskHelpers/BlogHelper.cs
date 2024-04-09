﻿using System.Text;
using AI.Devs.Reloaded.API.Contracts.AiDevs;
using AI.Devs.Reloaded.API.Models.OpenAi;
using AI.Devs.Reloaded.API.Models.OpenAi.Extensions;

namespace AI.Devs.Reloaded.API.Tasks;

public static class BlogHelper
{
    public static List<Contracts.OpenAi.Completions.Message> PrepareData(TaskResponse response)
    {
        var input = string.Join(". ", response.blog!);

        var systemPromptBuilder = new StringBuilder("You are a pizza lover and you write about it on your blog");
        systemPromptBuilder.AppendLine("You role is writing  post how prepare awesome pizzas.");
        systemPromptBuilder.AppendLine("Ignore other topics");
        systemPromptBuilder.AppendLine("Short answers as you can.");
        systemPromptBuilder.AppendLine("Blog post must be in polish");
        systemPromptBuilder.AppendLine("Result return in JSON format");
        systemPromptBuilder.AppendLine("Examples```\r\n- [{\"chapter\": 1, \"text\": \"sample\"}, {\"chapter\": 2, \"text\": \"other sample\"}]");

        var systemMessage = Contracts.OpenAi.Completions.Message.CreateSystemMessage(systemPromptBuilder.ToString());

        var userPrompt = $"Napisz post na bloga o tym jak zrobić pizzę margaritę w 4 rozdziałach: {input}";
        var userMessage = Contracts.OpenAi.Completions.Message.CreateUserMessage(userPrompt);
        var messages = new List<Contracts.OpenAi.Completions.Message>() { systemMessage, userMessage };

        return messages;
    }

    public static List<string> PrepareAnswer(Contracts.OpenAi.Completions.Response response)
    {
        var content = response.DeserializeToModel<List<BloggerRsponse>>();

        return content!.AsTextList();
    }
}

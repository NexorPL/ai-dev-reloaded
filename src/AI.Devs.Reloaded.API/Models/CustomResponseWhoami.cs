using AI.Devs.Reloaded.API.Contracts.AiDevs.Answer;

namespace AI.Devs.Reloaded.API.Models;

public sealed class CustomResponseWhoami(int code, string msg, string? note, int counter, string answer)
{
    public int Code { get; } = code;
    public string Msg { get; } = msg;
    public string? Note { get; } = note;
    public int Counter { get; } = counter;
    public string Answer { get; } = answer;

    public static CustomResponseWhoami CreateFromAnswerResponse(AnswerResponse response, int counter, string answer)
    {
        return new CustomResponseWhoami(response.code, response.msg, response.note, counter, answer);
    }
}

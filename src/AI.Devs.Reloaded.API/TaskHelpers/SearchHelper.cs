using System.Text.Json;
using AI.Devs.Reloaded.API.Models;

namespace AI.Devs.Reloaded.API.TaskHelpers;

public static class SearchHelper
{
    public static async Task<List<ArchiveAiDevs>> ConvertToListArchiveAiDevs(Stream stream, CancellationToken cancellationToken)
    {
        using var reader = new StreamReader(stream);
        var archiveAiDevsJson = await reader.ReadToEndAsync(cancellationToken);

        return JsonSerializer.Deserialize<List<ArchiveAiDevs>>(archiveAiDevsJson)!;
    }
}

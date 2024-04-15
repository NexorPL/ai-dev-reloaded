namespace AI.Devs.Reloaded.API.HttpClients.Abstractions;

public interface IRenderFormClient
{
    Task<Contracts.RenderForm.Response> RenderFormAsync(Contracts.RenderForm.Request request, CancellationToken ct = default);
}

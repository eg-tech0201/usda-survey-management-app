using app_models.Workflows;

namespace app_services.Contracts;

public interface IRespondentWorkspaceService
{
    Task<RespondentDetailDto?> GetRespondentDetailAsync(
        RespondentKey key,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<InteractionTimelineEventDto>> GetInteractionTimelineAsync(
        RespondentKey key,
        CancellationToken cancellationToken = default);
}

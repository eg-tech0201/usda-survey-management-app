using app_models.Workflows;

namespace app_services.Contracts.Integration;

public interface ISurveyReviewGateway
{
    Task<IReadOnlyList<QuestionnaireArtifactDto>> GetQuestionnairesAsync(
        SurveyInstanceKey key,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<CollectionMaterialDto>> GetCollectionMaterialsAsync(
        SurveyInstanceKey key,
        CancellationToken cancellationToken = default);

    Task<DocumentPayloadDto?> GetDocumentAsync(
        DocumentRequestDto request,
        CancellationToken cancellationToken = default);
}

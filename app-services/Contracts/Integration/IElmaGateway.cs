using app_models.Integrations;

namespace app_services.Contracts.Integration;

public interface IElmaGateway
{
    Task<ElmaFoUpdateLinkResponse> GetFoUpdateRequestLinkAsync(CancellationToken cancellationToken = default);
    Task<ElmaSubmitTransactionResponse> SubmitTransactionAsync(ElmaSubmitTransactionRequest request, CancellationToken cancellationToken = default);
    Task<ElmaTransactionStatusResponse> GetTransactionStatusAsync(string transactionId, CancellationToken cancellationToken = default);
    Task<ElmaClientCapabilityResponse> GetCapabilitiesAsync(CancellationToken cancellationToken = default);
}

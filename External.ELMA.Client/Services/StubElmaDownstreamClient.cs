using External.ELMA.Client.Configuration;
using app_models.Integrations;
using Microsoft.Extensions.Options;

namespace External.ELMA.Client.Services;

public sealed class StubElmaDownstreamClient : IElmaDownstreamClient
{
    private readonly IOptionsMonitor<ElmaClientOptions> _options;
    private readonly Dictionary<string, ElmaTransactionStatusResponse> _transactions = new(StringComparer.OrdinalIgnoreCase);

    public StubElmaDownstreamClient(IOptionsMonitor<ElmaClientOptions> options)
    {
        _options = options;
    }

    public Task<ElmaSubmitTransactionResponse> SubmitTransactionAsync(ElmaSubmitTransactionRequest request, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var transactionId = $"ELMA-{DateTime.UtcNow:yyyyMMddHHmmss}-{Random.Shared.Next(1000, 9999)}";
        var submittedAt = DateTime.UtcNow;
        var response = new ElmaSubmitTransactionResponse(
            transactionId,
            ElmaTransactionStatus.Submitted,
            "Contact update request accepted by the ELMA client shell.",
            submittedAt,
            request.CorrelationId);

        _transactions[transactionId] = new ElmaTransactionStatusResponse(
            transactionId,
            ElmaTransactionStatus.Submitted,
            "The transaction has been queued for downstream ELMA delivery.",
            submittedAt,
            request.CorrelationId,
            null);

        return Task.FromResult(response);
    }

    public Task<ElmaTransactionStatusResponse> GetTransactionStatusAsync(string transactionId, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        if (_transactions.TryGetValue(transactionId, out var status))
            return Task.FromResult(status);

        return Task.FromResult(new ElmaTransactionStatusResponse(
            transactionId,
            ElmaTransactionStatus.Pending,
            "Transaction not found in the demo ELMA store.",
            DateTime.UtcNow,
            null,
            null));
    }

    public Task<ElmaFoUpdateLinkResponse> GetFoUpdateRequestLinkAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var options = _options.CurrentValue;
        var uri = new Uri(new Uri(options.BaseUrl), options.FoUpdateRequestPath);

        return Task.FromResult(new ElmaFoUpdateLinkResponse(
            uri,
            options.SupportsFormPrefill ? "prefill" : "static-link",
            options.SupportsFormPrefill
                ? "ELMA prefill is enabled in configuration."
                : "Current approved behavior is the static ELMA FO Update Request link."));
    }
}

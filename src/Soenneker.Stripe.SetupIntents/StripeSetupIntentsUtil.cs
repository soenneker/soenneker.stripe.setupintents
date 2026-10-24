using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Stripe.Client;
using Soenneker.Stripe.Client.Abstract;
using Soenneker.Stripe.OpenApiClient.Models;
using Soenneker.Stripe.SetupIntents.Abstract;

namespace Soenneker.Stripe.SetupIntents;

public sealed class StripeSetupIntentsUtil(IStripeClientUtil client) : IStripeSetupIntentsUtil
{
    public async ValueTask<SetupIntent> Create(string customerId, Soenneker.Stripe.Enums.SetupIntentUsage.SetupIntentUsage? usage,
        bool confirm = false, string? paymentMethodId = null, string? returnUrl = null, PostSetupIntentsXWwwFormUrlencodedRequestMandateData? mandateOptions = null,
        string? idempotencyKey = null, CancellationToken cancellationToken = default)
    {
        var options = new PostSetupIntentsXWwwFormUrlencodedRequest
        {
            Customer = customerId, Confirm = confirm, PaymentMethod = paymentMethodId, ReturnUrl = returnUrl,
            Usage = (usage ?? Soenneker.Stripe.Enums.SetupIntentUsage.SetupIntentUsage.OffSession).Value == "on_session"
                ? PostSetupIntentsXWwwFormUrlencodedRequestUsage.OnSession : PostSetupIntentsXWwwFormUrlencodedRequestUsage.OffSession,
            MandateData = mandateOptions, AutomaticPaymentMethods = new() { Enabled = true }
        };
        var api = await client.Get(cancellationToken);
        return await api.V1.Setup_intents.PostAsync(options, c => new StripeRequestOptions { IdempotencyKey = idempotencyKey }.Apply(c.Headers), cancellationToken)
            ?? throw new InvalidOperationException("Stripe returned an empty setup intent.");
    }

    public ValueTask<SetupIntent> CreateAndConfirmForOffSessionCard(string customerId, string paymentMethodId, string? returnUrl = null,
        string? idempotencyKey = null, CancellationToken cancellationToken = default) =>
        Create(customerId, Soenneker.Stripe.Enums.SetupIntentUsage.SetupIntentUsage.OffSession, true, paymentMethodId, returnUrl, null, idempotencyKey, cancellationToken);

    public async ValueTask<SetupIntent> Confirm(string id, string? paymentMethodId = null, string? returnUrl = null, CancellationToken cancellationToken = default)
    {
        var api = await client.Get(cancellationToken);
        return await api.V1.Setup_intents[id].Confirm.PostAsync(new() { PaymentMethod = paymentMethodId, ReturnUrl = returnUrl }, cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Stripe returned an empty setup intent.");
    }
    public async ValueTask<SetupIntent> Get(string id, CancellationToken cancellationToken = default)
    {
        var api = await client.Get(cancellationToken);
        return await api.V1.Setup_intents[id].GetAsync(new(), cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Stripe returned an empty intent.");
    }

    public async ValueTask<SetupIntent> Update(string id, Dictionary<string, string> metadata, CancellationToken cancellationToken = default)
    {
        var options = new PostSetupIntentsIntentXWwwFormUrlencodedRequest();
        options.Metadata = new() { PostSetupIntentsIntentXWwwFormUrlencodedRequestMetadataAnyOf1 = new() { AdditionalData = metadata.ToDictionary(pair => pair.Key, pair => (object)pair.Value) } };
        var api = await client.Get(cancellationToken);
        return await api.V1.Setup_intents[id].PostAsync(options, cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Stripe returned an empty intent.");
    }

    public async ValueTask<IEnumerable<SetupIntent>> List(string customerId, CancellationToken cancellationToken = default)
    {
        var api = await client.Get(cancellationToken);
        var result = new List<SetupIntent>();
        string? cursor = null;
        do
        {
            var page = await api.V1.Setup_intents.GetAsync(new(), c => { c.QueryParameters.Customer = customerId; c.QueryParameters.Limit = 100; c.QueryParameters.StartingAfter = cursor; }, cancellationToken);
            if (page?.Data is not { Count: > 0 }) break;
            result.AddRange(page.Data);
            cursor = page.HasMore == true ? page.Data[^1].Id : null;
        } while (cursor != null);
        return result;
    }
    public async ValueTask<SetupIntent> Cancel(string id, CancellationToken cancellationToken = default)
    {
        var api = await client.Get(cancellationToken);
        return await api.V1.Setup_intents[id].Cancel.PostAsync(new(), cancellationToken: cancellationToken)
            ?? throw new InvalidOperationException("Stripe returned an empty intent.");
    }
    public void Dispose() { }
    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}

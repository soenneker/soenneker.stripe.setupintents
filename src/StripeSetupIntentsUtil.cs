using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Extensions.Task;
using Soenneker.Extensions.ValueTask;
using Soenneker.Stripe.Client.Abstract;
using Soenneker.Stripe.SetupIntents.Abstract;
using Soenneker.Utils.AsyncSingleton;
using Stripe;
using Soenneker.Extensions.String;
using Soenneker.Stripe.SetupIntents.Enums;

namespace Soenneker.Stripe.SetupIntents;

/// <inheritdoc cref="IStripeSetupIntentsUtil"/>
public sealed class StripeSetupIntentsUtil : IStripeSetupIntentsUtil
{
    private readonly AsyncSingleton<SetupIntentService> _service;

    public StripeSetupIntentsUtil(IStripeClientUtil stripeUtil)
    {
        _service = new AsyncSingleton<SetupIntentService>(async (cancellationToken, _) =>
        {
            StripeClient client = await stripeUtil.Get(cancellationToken).NoSync();
            return new SetupIntentService(client);
        });
    }

    public async ValueTask<SetupIntent> Create(string customerId, SetupIntentUsage? usage, bool confirm = false, string? paymentMethodId = null,
        string? returnUrl = null, SetupIntentMandateDataOptions? mandateOptions = null, string? idempotencyKey = null,
        CancellationToken cancellationToken = default)
    {
        usage ??= SetupIntentUsage.OffSession;

        var options = new SetupIntentCreateOptions
        {
            Customer = customerId,
            Usage = usage,
            Confirm = confirm,
            ReturnUrl = returnUrl,
            PaymentMethod = paymentMethodId,
            MandateData = mandateOptions,
            AutomaticPaymentMethods = new SetupIntentAutomaticPaymentMethodsOptions {Enabled = true}
        };

        SetupIntentService service = await _service.Get(cancellationToken).NoSync();

        if (idempotencyKey.IsNullOrWhiteSpace())
            return await service.CreateAsync(options, cancellationToken: cancellationToken).NoSync();

        var requestOptions = new RequestOptions
        {
            IdempotencyKey = idempotencyKey
        };

        return await service.CreateAsync(options, requestOptions, cancellationToken).NoSync();
    }

    public ValueTask<SetupIntent> CreateAndConfirmForOffSessionCard(string customerId, string paymentMethodId, string? returnUrl = null,
        string? idempotencyKey = null, CancellationToken cancellationToken = default)
    {
        return Create(customerId, usage: SetupIntentUsage.OffSession, confirm: true, paymentMethodId: paymentMethodId, returnUrl: returnUrl,
            mandateOptions: null, idempotencyKey: idempotencyKey, cancellationToken);
    }

    public async ValueTask<SetupIntent> Get(string id, CancellationToken cancellationToken = default)
    {
        SetupIntentService service = await _service.Get(cancellationToken).NoSync();
        return await service.GetAsync(id, cancellationToken: cancellationToken).NoSync();
    }

    public async ValueTask<SetupIntent> Cancel(string id, CancellationToken cancellationToken = default)
    {
        SetupIntentService service = await _service.Get(cancellationToken).NoSync();
        return await service.CancelAsync(id, cancellationToken: cancellationToken).NoSync();
    }

    public async ValueTask<SetupIntent> Confirm(string id, string? paymentMethodId = null, string? returnUrl = null,
        CancellationToken cancellationToken = default)
    {
        var options = new SetupIntentConfirmOptions
        {
            PaymentMethod = paymentMethodId,
            ReturnUrl = returnUrl
        };

        SetupIntentService service = await _service.Get(cancellationToken).NoSync();
        return await service.ConfirmAsync(id, options, cancellationToken: cancellationToken).NoSync();
    }

    public async ValueTask<SetupIntent> Update(string id, Dictionary<string, string> metadata, CancellationToken cancellationToken = default)
    {
        var options = new SetupIntentUpdateOptions
        {
            Metadata = metadata
        };

        SetupIntentService service = await _service.Get(cancellationToken).NoSync();
        return await service.UpdateAsync(id, options, cancellationToken: cancellationToken).NoSync();
    }

    public async ValueTask<IEnumerable<SetupIntent>> List(string customerId, CancellationToken cancellationToken = default)
    {
        var options = new SetupIntentListOptions
        {
            Customer = customerId,
            Limit = 100
        };

        SetupIntentService service = await _service.Get(cancellationToken).NoSync();
        return await service.ListAsync(options, cancellationToken: cancellationToken).NoSync();
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _service.Dispose();
    }

    public ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);
        return _service.DisposeAsync();
    }
}
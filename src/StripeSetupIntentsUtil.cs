using Soenneker.Stripe.Client.Abstract;
using Soenneker.Stripe.SetupIntents.Abstract;
using Soenneker.Utils.AsyncSingleton;
using Stripe;
using System;
using System.Threading;
using System.Threading.Tasks;
using Soenneker.Extensions.Task;
using Soenneker.Extensions.ValueTask;

namespace Soenneker.Stripe.SetupIntents;

///<inheritdoc cref="IStripeSetupIntentsUtil"/>
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

    public async ValueTask<SetupIntent> Create(string customerId, CancellationToken cancellationToken = default)
    {
        var options = new SetupIntentCreateOptions
        {
            Customer = customerId,
            Usage = "off_session"
        };

        return await (await _service.Get(cancellationToken).NoSync()).CreateAsync(options, cancellationToken: cancellationToken).NoSync();
    }

    public async ValueTask<SetupIntent> Get(string id, CancellationToken cancellationToken = default)
    {
        return await (await _service.Get(cancellationToken).NoSync()).GetAsync(id, cancellationToken: cancellationToken).NoSync();
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
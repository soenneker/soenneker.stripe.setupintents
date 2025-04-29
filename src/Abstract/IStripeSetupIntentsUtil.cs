using Stripe;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Stripe.SetupIntents.Abstract;

/// <summary>
/// A .NET typesafe implementation of Stripe's Setup Intents API
/// </summary>
public interface IStripeSetupIntentsUtil : IAsyncDisposable, IDisposable
{
    /// <summary>
    /// Creates a new <see cref="SetupIntent"/> for the specified customer, allowing a payment method to be saved for future use.
    /// </summary>
    /// <param name="customerId">The ID of the Stripe customer to associate the SetupIntent with.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The newly created <see cref="SetupIntent"/>.</returns>
    ValueTask<SetupIntent> Create(string customerId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a <see cref="SetupIntent"/> by its ID.
    /// </summary>
    /// <param name="id">The ID of the SetupIntent to retrieve.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>The retrieved <see cref="SetupIntent"/>.</returns>
    ValueTask<SetupIntent> Get(string id, CancellationToken cancellationToken = default);
}
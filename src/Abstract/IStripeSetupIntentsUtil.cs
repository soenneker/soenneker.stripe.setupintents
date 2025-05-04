using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Stripe;

namespace Soenneker.Stripe.SetupIntents.Abstract;

/// <summary>
/// A utility for working with Stripe SetupIntents. Used for saving and validating payment methods (e.g., cards, bank accounts) for future off-session use.
/// </summary>
public interface IStripeSetupIntentsUtil : IAsyncDisposable, IDisposable
{
    /// <summary>
    /// Creates a new SetupIntent for a customer.
    /// </summary>
    /// <param name="customerId">The Stripe customer ID for whom the SetupIntent is being created.</param>
    /// <param name="confirm">Whether to immediately confirm the SetupIntent and validate the payment method.</param>
    /// <param name="paymentMethodId">Optional payment method ID to attach and verify during confirmation.</param>
    /// <param name="usage">Specifies the usage of the payment method. Use "off_session" for future, background charges.</param>
    /// <param name="returnUrl">Optional return URL for redirect-based confirmations (e.g., 3D Secure flows).</param>
    /// <param name="mandateOptions">Optional mandate data for collecting authorization for recurring or bank payments.</param>
    /// <param name="idempotencyKey"></param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The created SetupIntent object.</returns>
    ValueTask<SetupIntent> Create(string customerId, bool confirm = false, string? paymentMethodId = null, string? usage = "off_session",
        string? returnUrl = null, SetupIntentMandateDataOptions? mandateOptions = null, string? idempotencyKey = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves an existing SetupIntent by its ID.
    /// </summary>
    /// <param name="id">The ID of the SetupIntent to retrieve.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The retrieved SetupIntent.</returns>
    ValueTask<SetupIntent> Get(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancels a SetupIntent that has not yet been confirmed.
    /// </summary>
    /// <param name="id">The ID of the SetupIntent to cancel.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The canceled SetupIntent object.</returns>
    ValueTask<SetupIntent> Cancel(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Confirms a SetupIntent manually with an optional payment method and return URL.
    /// </summary>
    /// <param name="id">The ID of the SetupIntent to confirm.</param>
    /// <param name="paymentMethodId">Optional payment method ID to confirm with.</param>
    /// <param name="returnUrl">Optional return URL for redirect-based authentication.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The confirmed SetupIntent object.</returns>
    ValueTask<SetupIntent> Confirm(string id, string? paymentMethodId = null, string? returnUrl = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates metadata on an existing SetupIntent.
    /// </summary>
    /// <param name="id">The ID of the SetupIntent to update.</param>
    /// <param name="metadata">Key-value pairs to attach as metadata.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>The updated SetupIntent object.</returns>
    ValueTask<SetupIntent> Update(string id, Dictionary<string, string> metadata, CancellationToken cancellationToken = default);

    /// <summary>
    /// Lists all SetupIntents associated with a given customer.
    /// </summary>
    /// <param name="customerId">The Stripe customer ID to filter SetupIntents by.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A collection of SetupIntents for the specified customer.</returns>
    ValueTask<IEnumerable<SetupIntent>> List(string customerId, CancellationToken cancellationToken = default);
}
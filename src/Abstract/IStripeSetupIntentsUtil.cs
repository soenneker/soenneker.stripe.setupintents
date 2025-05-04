using Soenneker.Stripe.SetupIntents.Enums;
using Stripe;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Stripe.SetupIntents.Abstract;

/// <summary>
/// A utility for managing Stripe SetupIntents. Used to securely collect and store payment methods (e.g., cards, ACH) for future use, including off-session billing.
/// </summary>
public interface IStripeSetupIntentsUtil : IAsyncDisposable, IDisposable
{
    /// <summary>
    /// Creates a new SetupIntent for a given customer, optionally confirming and validating a specified payment method.
    /// </summary>
    /// <param name="customerId">The ID of the Stripe customer the SetupIntent will be associated with.</param>
    /// <param name="usage">
    /// Indicates how the payment method will be used once saved. 
    /// Use <c>off_session</c> for recurring or background charges, and <c>on_session</c> for interactive checkout flows.
    /// </param>
    /// <param name="confirm">If true, the SetupIntent will be confirmed immediately, triggering validation of the attached payment method.</param>
    /// <param name="paymentMethodId">Optional Stripe payment method ID to attach and validate during confirmation.</param>
    /// <param name="returnUrl">Optional URL to redirect the customer after confirmation (required for some 3D Secure flows).</param>
    /// <param name="mandateOptions">Optional mandate information for payment methods that require customer authorization (e.g., bank debits).</param>
    /// <param name="idempotencyKey">Optional idempotency key to prevent duplicate SetupIntent creation if the request is retried.</param>
    /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
    /// <returns>The created SetupIntent instance.</returns>
    ValueTask<SetupIntent> Create(string customerId, SetupIntentUsage? usage, bool confirm = false, string? paymentMethodId = null, string? returnUrl = null,
        SetupIntentMandateDataOptions? mandateOptions = null, string? idempotencyKey = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates and immediately confirms a SetupIntent for a card payment method, configured for future off-session charges.
    /// </summary>
    /// <param name="customerId">The Stripe customer ID to associate the SetupIntent with.</param>
    /// <param name="paymentMethodId">The ID of the Stripe payment method to attach and validate.</param>
    /// <param name="returnUrl">Optional redirect URL for any required 3D Secure authentication steps.</param>
    /// <param name="idempotencyKey">Optional idempotency key to avoid duplicated SetupIntents.</param>
    /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
    /// <returns>The confirmed SetupIntent instance.</returns>
    ValueTask<SetupIntent> CreateAndConfirmForOffSessionCard(string customerId, string paymentMethodId, string? returnUrl = null, string? idempotencyKey = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a SetupIntent by its unique Stripe ID.
    /// </summary>
    /// <param name="id">The ID of the SetupIntent to retrieve.</param>
    /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
    /// <returns>The retrieved SetupIntent instance.</returns>
    ValueTask<SetupIntent> Get(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancels a SetupIntent that has not been confirmed. This is useful for abandoned or invalidated setup attempts.
    /// </summary>
    /// <param name="id">The ID of the SetupIntent to cancel.</param>
    /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
    /// <returns>The canceled SetupIntent instance.</returns>
    ValueTask<SetupIntent> Cancel(string id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Confirms an existing SetupIntent with an optional new payment method and return URL.
    /// </summary>
    /// <param name="id">The ID of the SetupIntent to confirm.</param>
    /// <param name="paymentMethodId">Optional new payment method ID to attach at the time of confirmation.</param>
    /// <param name="returnUrl">Optional URL to redirect the customer after confirmation (used for 3D Secure or similar flows).</param>
    /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
    /// <returns>The confirmed SetupIntent instance.</returns>
    ValueTask<SetupIntent> Confirm(string id, string? paymentMethodId = null, string? returnUrl = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing SetupIntent with metadata.
    /// </summary>
    /// <param name="id">The ID of the SetupIntent to update.</param>
    /// <param name="metadata">A dictionary of key-value pairs to attach as metadata to the SetupIntent.</param>
    /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
    /// <returns>The updated SetupIntent instance.</returns>
    ValueTask<SetupIntent> Update(string id, Dictionary<string, string> metadata, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a list of SetupIntents for a given customer.
    /// </summary>
    /// <param name="customerId">The Stripe customer ID to filter SetupIntents by.</param>
    /// <param name="cancellationToken">Token to observe while waiting for the task to complete.</param>
    /// <returns>An enumerable collection of SetupIntent instances.</returns>
    ValueTask<IEnumerable<SetupIntent>> List(string customerId, CancellationToken cancellationToken = default);
}
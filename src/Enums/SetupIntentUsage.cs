using Intellenum;

namespace Soenneker.Stripe.SetupIntents.Enums;

/// <summary>
/// Indicates how the payment method will be used once saved. Used for Stripe SetupIntents.
/// </summary>
[Intellenum<string>]
public partial class SetupIntentUsage
{
    /// <summary>
    /// The payment method will be used immediately while the customer is present.
    /// </summary>
    public static readonly SetupIntentUsage OnSession = new("on_session");

    /// <summary>
    /// The payment method will be used later without the customer present (e.g., auto-recharges).
    /// </summary>
    public static readonly SetupIntentUsage OffSession = new("off_session");
}
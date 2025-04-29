using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.Stripe.Client.Registrars;
using Soenneker.Stripe.SetupIntents.Abstract;

namespace Soenneker.Stripe.SetupIntents.Registrars;

/// <summary>
/// A .NET typesafe implementation of Stripe's Setup Intents API
/// </summary>
public static class StripeSetupIntentsUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="IStripeSetupIntentsUtil"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddStripeSetupIntentsUtilAsSingleton(this IServiceCollection services)
    {
        services.AddStripeClientUtilAsSingleton().TryAddSingleton<IStripeSetupIntentsUtil, StripeSetupIntentsUtil>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="IStripeSetupIntentsUtil"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddStripeSetupIntentsUtilAsScoped(this IServiceCollection services)
    {
        services.AddStripeClientUtilAsSingleton().TryAddScoped<IStripeSetupIntentsUtil, StripeSetupIntentsUtil>();

        return services;
    }
}

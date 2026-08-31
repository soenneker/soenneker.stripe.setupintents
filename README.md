[![](https://img.shields.io/nuget/v/soenneker.stripe.setupintents.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.stripe.setupintents/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.stripe.setupintents/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.stripe.setupintents/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.stripe.setupintents.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.stripe.setupintents/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.stripe.setupintents/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.stripe.setupintents/actions/workflows/codeql.yml)

# ![](https://user-images.githubusercontent.com/4441470/224455560-91ed3ee7-f510-4041-a8d2-3fc093025112.png) Soenneker.Stripe.SetupIntents

Create, confirm, retrieve, update, cancel, and list Stripe setup intents for saving payment methods for later use.

## Installation

```bash
dotnet add package Soenneker.Stripe.SetupIntents
```

## Configuration

```json
{
  "Stripe": {
    "SecretKey": "sk_test_..."
  }
}
```

## Usage

```csharp
using Soenneker.Stripe.SetupIntents.Abstract;
using Soenneker.Stripe.SetupIntents.Registrars;
using Stripe;

services.AddStripeSetupIntentsUtilAsScoped();

SetupIntent setupIntent = await setupIntentsUtil.CreateAndConfirmForOffSessionCard(
    customerId: "cus_...",
    paymentMethodId: "pm_...",
    returnUrl: "https://example.com/billing/setup-complete",
    idempotencyKey: $"save-payment-method-{customerId}",
    cancellationToken: cancellationToken);
```

`Create` defaults to off-session usage and enables automatic payment methods. `CreateAndConfirmForOffSessionCard` assumes the supplied payment-method ID represents a card; Stripe validates the actual method and may require customer action through the return URL.

`List` returns at most the first 100 setup intents for a customer. Create, confirm, update, and cancel calls change Stripe state, and Stripe API errors propagate to the caller.

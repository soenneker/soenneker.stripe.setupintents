using Soenneker.Stripe.SetupIntents.Abstract;
using Soenneker.Tests.HostedUnit;

namespace Soenneker.Stripe.SetupIntents.Tests;

[ClassDataSource<Host>(Shared = SharedType.PerTestSession)]
public class StripeSetupIntentsUtilTests : HostedUnitTest
{
    private readonly IStripeSetupIntentsUtil _util;

    public StripeSetupIntentsUtilTests(Host host) : base(host)
    {
        _util = Resolve<IStripeSetupIntentsUtil>(true);
    }

    [Test]
    public void Default()
    {

    }
}

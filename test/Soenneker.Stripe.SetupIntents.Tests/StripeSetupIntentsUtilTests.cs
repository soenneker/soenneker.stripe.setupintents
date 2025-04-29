using Soenneker.Stripe.SetupIntents.Abstract;
using Soenneker.Tests.FixturedUnit;
using Xunit;

namespace Soenneker.Stripe.SetupIntents.Tests;

[Collection("Collection")]
public class StripeSetupIntentsUtilTests : FixturedUnitTest
{
    private readonly IStripeSetupIntentsUtil _util;

    public StripeSetupIntentsUtilTests(Fixture fixture, ITestOutputHelper output) : base(fixture, output)
    {
        _util = Resolve<IStripeSetupIntentsUtil>(true);
    }

    [Fact]
    public void Default()
    {

    }
}

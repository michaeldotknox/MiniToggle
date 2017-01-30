using MiniToggle.Core;
using MiniToggle.UnitTests.Toggles;
using NUnit.Framework;

namespace MiniToggle.UnitTests
{
    [SetUpFixture]
    public class SetupFixture
    {
        public SetupFixture()
        {
            Toggle.Init();

            Toggle<AlwaysTrueToggle>.Is().AlwaysTrue();
            Toggle<AlwaysFalseToggle>.Is().AlwaysFalse();
        }
    }
}

using MiniToggle.Core;
using NUnit.Framework;

namespace MiniToggle.UnitTests
{
    [SetUpFixture]
    public class SetupFixture
    {
        [SetUp]
        public void Setup()
        {
            Toggle.Init();
        }
    }
}

using MiniToggle.Core;
using MiniToggle.Core.Attributes;

namespace MiniToggle.UnitTests.Toggles
{
    [AlwaysFalse]
    public class AlwaysFalseWithAttribute : IToggle
    {
    }
}

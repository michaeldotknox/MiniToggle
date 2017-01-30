using MiniToggle.Core;
using MiniToggle.Core.Attributes;

namespace MiniToggle.TestProject.Toggles
{
    [SettingConfiguration("testToggle")]
    public class AppConfigWithAttribute : IToggle
    {
    }
}

using MiniToggle.Core;
using MiniToggle.Core.Attributes;

namespace TestProject.Toggles
{
    [SettingConfiguration("testToggle")]
    public class AppConfigWithAttribute : IToggle
    {
    }
}

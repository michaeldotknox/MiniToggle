using System;

namespace MiniToggle.Core
{
    /// <summary>
    /// Holds information on a toggle that is configured with a settings file such as a web.config or app.config
    /// </summary>
    public class SettingFileConfiguration
    {
        internal Type Toggle { get; set; }
        internal string SettingName { get; set; }
    }
}
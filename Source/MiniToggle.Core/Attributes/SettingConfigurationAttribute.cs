using System;

namespace MiniToggle.Core.Attributes
{
    /// <summary>
    /// Indicates that the toggle is configured with the given setting
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SettingConfigurationAttribute : Attribute
    {
        internal string SettingName { get; private set; }

        /// <summary>
        /// Creates an attribute that indicates that the toggle is configured with the setting
        /// </summary>
        /// <param name="settingName">The name of the setting used to configure the toggle</param>
        public SettingConfigurationAttribute(string settingName)
        {
            SettingName = settingName;
        }
    }
}

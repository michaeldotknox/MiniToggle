using System;
using System.Configuration;

namespace MiniToggle.Core.Attributes
{
    /// <summary>
    /// Indicates that the toggle is configured with the given setting
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class SettingConfigurationAttribute : ToggleAttribute
    {
        internal string SettingName { get; }
        private bool DefaultValue { get; }

        /// <summary>
        /// Creates an attribute that indicates that the toggle is configured with the setting
        /// </summary>
        /// <param name="settingName">The name of the setting used to configure the toggle</param>
        /// <param name="defaultValue">The default value to use if the setting is not found in the configuration file</param>
        public SettingConfigurationAttribute(string settingName, bool defaultValue)
        {
            SettingName = settingName;
            DefaultValue = defaultValue;
        }

        /// <summary>
        /// Creates an attribute that indicates that the toggle is configured with the setting
        /// </summary>
        /// <param name="settingName">The name of the setting used to configure the toggle</param>
        public SettingConfigurationAttribute(string settingName) : this(settingName, true)
        {
            
        }

        internal override ToggleDefinition GetDefinition(Type type)
        {
            return new ToggleDefinition
            {
                Type = type,
                Evaluation = () =>
                {
                    var setting = ConfigurationManager.AppSettings[SettingName];
                    if (setting == null)
                    {
                        return DefaultValue;
                    }

                    return ConfigurationManager.AppSettings[SettingName] == "true";
                }
            };
        }
    }
}

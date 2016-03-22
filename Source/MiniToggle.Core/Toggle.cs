using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace MiniToggle.Core
{
    /// <summary>
    /// Core toggle object for managing custom toggles
    /// </summary>
    /// <typeparam name="TToggle">The type of the custom toggle</typeparam>
    public static class Toggle<TToggle> where TToggle : IToggle
    {
        /// <summary>
        /// Retrieves configuration information for the toggle
        /// </summary>
        /// <returns>A <see cref="ToggleConfiguration"/> for the requested toggle</returns>
        public static ToggleConfiguration Is()
        {
            return new ToggleConfiguration {Toggle = typeof (TToggle)};
        }

        /// <summary>
        /// Determines if the given toggle is enabled
        /// </summary>
        /// <returns>A boolean indicating if the toggle is enabled</returns>
        public static bool IsEnabled()
        {
            return Toggle.Toggles[typeof(TToggle)].Invoke();
        }
    }

    /// <summary>
    /// Extension methods for managing toggles
    /// </summary>
    public static class Toggle
    {
        internal static readonly Dictionary<Type, Func<bool>> Toggles; 

        static Toggle()
        {
            Toggles =
                AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(
                        assembly => assembly.GetTypes().Where(type => type.GetInterfaces().Contains(typeof (IToggle))))
                    .ToDictionary(x => x, y => null as Func<bool>);
        }

        /// <summary>
        /// Marks a toggle as always true
        /// </summary>
        /// <param name="toggleConfiguration">The <see cref="ToggleConfiguration"/> for the toggle</param>
        public static void AlwaysTrue(this ToggleConfiguration toggleConfiguration)
        {
            Toggles[toggleConfiguration.Toggle] = () => true;
        }

        /// <summary>
        /// Marks the toggle as always false
        /// </summary>
        /// <param name="toggleConfiguration">The <see cref="ToggleConfiguration"/> for the toggle</param>
        public static void AlwaysFalse(this ToggleConfiguration toggleConfiguration)
        {
            Toggles[toggleConfiguration.Toggle] = () => false;
        }

        /// <summary>
        /// Marks a toggle as configured through an external method
        /// </summary>
        /// <param name="toggleConfiguration">The <see cref="ToggleConfiguration"/> for the toggle</param>
        /// <returns>A <see cref="ConfigurableToggle"/> for the toggle</returns>
        public static ConfigurableToggle Configured(this ToggleConfiguration toggleConfiguration)
        {
            return new ConfigurableToggle {Toggle = toggleConfiguration.Toggle};
        }

        /// <summary>
        /// Indicates that the toggle is configured with a setting from an app.config or web.config file
        /// </summary>
        /// <param name="configurableToggle">The <see cref="ConfigurableToggle"/> for the toggle</param>
        /// <returns>A <see cref="SettingFileConfiguration"/> for the toggle</returns>
        public static SettingFileConfiguration WithSetting(this ConfigurableToggle configurableToggle)
        {
            return new SettingFileConfiguration {Toggle = configurableToggle.Toggle};
        }

        /// <summary>
        /// Indicates that the toggle is configured with the given named setting
        /// </summary>
        /// <param name="settingFileConfiguration">The <see cref="SettingFileConfiguration"/> for the toggle</param>
        /// <param name="settingName">The name of the setting to use</param>
        public static void Named(this SettingFileConfiguration settingFileConfiguration, string settingName)
        {
            Toggles[settingFileConfiguration.Toggle] = () =>
            {
                var setting = ConfigurationManager.AppSettings[settingName];
                if (setting == null)
                {
                    return true;
                }

                return ConfigurationManager.AppSettings[settingName] == "true";
            };
        }
    }
}

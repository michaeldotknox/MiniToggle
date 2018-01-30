using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using MiniToggle.Core.Attributes;
using MiniToggle.Core.Exceptions;

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
            var enabled = Toggle.Toggles.First(toggle => toggle.Type == typeof (TToggle));
            if(enabled.Evaluation == null)
            {
                throw new ToggleNotConfiguredException(typeof(TToggle).Name);
            }

            return enabled.Evaluation.Invoke();
        }

        /// <summary>
        /// Executes the appropriate code based on the status of the toggle.  This can be an anonymous delegate or a predefined delegate.  
        /// Each of the delegates must return the same type.
        /// </summary>
        /// <param name="executeIfEnabled">The code to execute if the toggle is enabled</param>
        /// <param name="executeIfDisabled">The code to execute if the toggle is disabled</param>
        /// <typeparam name="TResult">The type returned by the methods</typeparam>
        /// <returns>The result returned by the methods</returns>
        public static TResult Execute<TResult>(Func<TResult> executeIfEnabled, Func<TResult> executeIfDisabled)
        {
            var enabled = IsEnabled();

            return enabled ? executeIfEnabled() : executeIfDisabled();
        }

        /// <summary>
        /// Executes the appropriate code based on the status of the toggle.  This can be an anonymous delegate or a predefined delegate.  
        /// </summary>
        /// <param name="executeIfEnabled">The code to execute if the toggle is enabled</param>
        /// <param name="executeIfDisabled">The code to execute if the toggle is disabled</param>
        public static void Execute(Action executeIfEnabled, Action executeIfDisabled)
        {
            var enabled = IsEnabled();

            if (enabled)
            {
                executeIfEnabled();
            }
            else
            {
                executeIfDisabled();
            }
        }

        /// <summary>
        /// Executes the appropriate code asynchronously based on the status of the toggle.  This can be an anonymous delegate or a predefined delegate.  
        /// Each of the delegates must return the same type.
        /// </summary>
        /// <param name="executeIfEnabled">The code to execute if the toggle is enabled</param>
        /// <param name="executeIfDisabled">The code to execute if the toggle is disabled</param>
        /// <typeparam name="TResult">The type returned by the methods</typeparam>
        /// <returns>The result returned by the methods</returns>
        public static async Task<TResult> ExecuteAsync<TResult>(Func<Task<TResult>> executeIfEnabled,
            Func<Task<TResult>> executeIfDisabled)
        {
            var enabled = IsEnabled();

            if (enabled)
            {
                return await executeIfEnabled();
            }
            return await executeIfDisabled();
        }

        /// <summary>
        /// Executes the appropriate code based on the status of the toggle.  This can be an anonymous delegate or a predefined delegate.  
        /// </summary>
        /// <param name="executeIfEnabled">The code to execute if the toggle is enabled</param>
        /// <param name="executeIfDisabled">The code to execute if the toggle is disabled</param>
        public static async Task ExecuteAsync(Func<Task> executeIfEnabled, Func<Task> executeIfDisabled)
        {
            var enabled = IsEnabled();

            if (enabled)
            {
                await executeIfEnabled();
            }
            else
            {
                await executeIfDisabled();
            }
        }
    }

    /// <summary>
    /// Extension methods for managing toggles
    /// </summary>
    public static class Toggle
    {
        internal static readonly List<ToggleDefinition> Toggles; 

        static Toggle()
        {
            // A list of all classes that implement the IToggle interface
            var toggles =
                AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(
                        GetTypes).ToList();

            // A list of predefined toggles for which we need to create the toggle definition
            var initializedToggles = GetToggles(toggles);

            Toggles = toggles.GroupJoin(initializedToggles, toggle => toggle, initializedToggle => initializedToggle.Type,
                (toggle, initializedToggle) => new { toggle, initializedToggle = initializedToggle.DefaultIfEmpty() })
                .Select(finalToggle => new ToggleDefinition { Type = finalToggle.toggle, Evaluation = finalToggle.initializedToggle.First()?.Evaluation }).ToList();
        }

        private static IEnumerable<Type> GetTypes(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes().Where(type => type.GetInterfaces().Contains(typeof(IToggle)));
            }
            catch
            {
                return new List<Type>();
                throw;
            }
        }

        private static IEnumerable<ToggleDefinition> GetToggles(IEnumerable<Type> toggles)
        {
            var attributedToggles = toggles.Where(toggle => toggle.GetCustomAttribute<ToggleAttribute>() != null);
            var definitions = attributedToggles.Select(toggle => toggle.GetCustomAttribute<ToggleAttribute>().GetDefinition(toggle));

            return definitions;
        }

        internal static void Init()
        {
        }

        /// <summary>
        /// Internal method to retrieve a toggle for testing
        /// </summary>
        /// <param name="toggleType">The type of the toggle sought</param>
        /// <returns>A <see cref="ToggleDefinition"/></returns>
        internal static ToggleDefinition GetToggle(Type toggleType)
        {
            return Toggles.First(toggle => toggle.Type == toggleType);
        }

        /// <summary>
        /// Marks a toggle as always true
        /// </summary>
        /// <param name="toggleConfiguration">The <see cref="ToggleConfiguration"/> for the toggle</param>
        public static void AlwaysTrue(this ToggleConfiguration toggleConfiguration)
        {
            SetEvalation(toggleConfiguration.Toggle, SetTrue());
        }

        /// <summary>
        /// Marks the toggle as always false
        /// </summary>
        /// <param name="toggleConfiguration">The <see cref="ToggleConfiguration"/> for the toggle</param>
        public static void AlwaysFalse(this ToggleConfiguration toggleConfiguration)
        {
            SetEvalation(toggleConfiguration.Toggle, SetFalse());
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
        /// Creates a cached toggle allowing the user to determine how the toggle is cached
        /// </summary>
        /// <param name="configurableToggle">A <see cref="ConfigurableToggle"/></param>
        /// <returns>A <see cref="CachedToggle"/></returns>
        public static CachedToggle Cached(this ToggleConfiguration configurableToggle)
        {
            if (configurableToggle.Toggle == null) throw new ToggleCannotBeNullException();

            return new CachedToggle {Toggle = configurableToggle.Toggle};
        }

        /// <summary>
        /// Initializes the cached toggle at initialization
        /// </summary>
        /// <param name="cachedToggle">A <see cref="CachedToggle"/></param>
        public static void AtInitialization(this CachedToggle cachedToggle)
        {
            var result = Toggles.Single(_ => _.Type == cachedToggle.Toggle).Evaluation();
            SetEvalation(cachedToggle.Toggle, Expression.Lambda<Func<bool>>(Expression.Constant(result)).Compile());
        }

        /// <summary>
        /// Indicates that the toggle is configured with the given named setting
        /// </summary>
        /// <param name="settingFileConfiguration">The <see cref="SettingFileConfiguration"/> for the toggle</param>
        /// <param name="settingName">The name of the setting to use</param>
        public static SettingFileConfiguration Named(this SettingFileConfiguration settingFileConfiguration, string settingName)
        {
            settingFileConfiguration.SettingName = settingName;
            SetEvalation(settingFileConfiguration.Toggle, () =>
            {
                var setting = ConfigurationManager.AppSettings[settingName];
                if (setting == null)
                {
                    return true;
                }

                return ConfigurationManager.AppSettings[settingName] == "true";
            });

            return settingFileConfiguration;
        }

        /// <summary>
        /// Sets the default value for the toggle if the the setting is not present in the config file
        /// </summary>
        /// <param name="settingFileConfiguration">The <see cref="SettingFileConfiguration"/> for the toggle</param>
        /// <param name="defaultValue">The default value to use if the setting is not present in the config file</param>
        public static void Default(this SettingFileConfiguration settingFileConfiguration, bool defaultValue)
        {
            SetEvalation(settingFileConfiguration.Toggle, () =>
            {
                var setting = ConfigurationManager.AppSettings[settingFileConfiguration.SettingName];
                if (setting == null)
                {
                    return defaultValue;
                }

                return ConfigurationManager.AppSettings[settingFileConfiguration.SettingName] == "true";
            });
        }

        /// <summary>
        /// Configures the toggle with a delegate
        /// </summary>
        /// <param name="configurableToggle">The <see cref="ConfigurableToggle"/> for the toggle</param>
        public static DelegateConfiguration With(this ConfigurableToggle configurableToggle)
        {
            return new DelegateConfiguration { Toggle = configurableToggle.Toggle };
        }

        /// <summary>
        /// Specifies the delegate to call to retrieve the toggle value
        /// </summary>
        /// <param name="delegateConfiguration">The delegate configuration</param>
        /// <param name="evaluation">The delegate to call to retrieve the toggle's configuration</param>
        /// <returns>A <see cref="Delegate"/></returns>
        public static DelegateConfiguration Delegate(this DelegateConfiguration delegateConfiguration, Func<bool> evaluation)
        {
            SetEvalation(delegateConfiguration.Toggle, evaluation);
            return delegateConfiguration;
        }

        private static void SetEvalation(Type type, Func<bool> evaluation)
        {
            var toggle = Toggles.FirstOrDefault(soughtToggle => soughtToggle.Type == type);

            if (toggle == null)
            {
                throw new ToggleNotConfiguredException(type.Name);
            }
            toggle.Evaluation = evaluation;
        }

        private static Func<bool> SetTrue()
        {
            return () => true;
        }

        private static Func<bool> SetFalse()
        {
            return () => false;
        }
    }
}

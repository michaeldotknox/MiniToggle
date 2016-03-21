﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace MiniToggle.Core
{
    public static class Toggle<TToggle> where TToggle : IToggle
    {
        public static ToggleConfiguration Is()
        {
            return new ToggleConfiguration {Toggle = typeof (TToggle)};
        }

        public static bool IsEnabled()
        {
            return Toggle.Toggles[typeof(TToggle)].Invoke();
        }
    }

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

        public static void AlwaysTrue(this ToggleConfiguration toggleConfiguration)
        {
            Toggles[toggleConfiguration.Toggle] = () => true;
        }

        public static void AlwaysFalse(this ToggleConfiguration toggleConfiguration)
        {
            Toggles[toggleConfiguration.Toggle] = () => false;
        }

        public static ConfigurableToggle Configured(this ToggleConfiguration toggleConfiguration)
        {
            return new ConfigurableToggle {Toggle = toggleConfiguration.Toggle};
        }

        public static ApiConfiguration WithSetting(this ConfigurableToggle configurableToggle)
        {
            return new ApiConfiguration {Toggle = configurableToggle.Toggle};
        }

        public static void Named(this ApiConfiguration apiConfiguration, string settingName)
        {
            Toggles[apiConfiguration.Toggle] = () =>
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

using System;
using MiniToggle.Core;
using TestProject.Toggles;

namespace TestProject
{
    class Program
    {
        static void Main()
        {
            // Initialize toggles
            Toggle<AlwaysTrue>.Is().AlwaysTrue();
            Toggle<AlwaysFalse>.Is().AlwaysFalse();
            Toggle<AppConfig>.Is().Configured().WithSetting().Named("testToggle");

            Console.WriteLine(Toggle<AlwaysTrue>.IsEnabled()
                ? "Always true toggle is true.  This should be called."
                : "Always true toggle is false.  This should not be called.");

            Console.WriteLine(Toggle<AlwaysFalse>.IsEnabled()
                ? "Always false toggle is true.  This should not be called."
                : "Always false toggle is false.  This should be called.");

            Console.WriteLine(Toggle<AppConfig>.IsEnabled()
                ? "Configuration toggle is true.  This should be called."
                : "Configuration toggle is false.  This should not be called.");

            Console.WriteLine(Toggle<AlwaysTrueWithAttribute>.IsEnabled()
                ? "Always true with attribute is true.  This should be called."
                : "Always true with attribute is false.  This should not be called.");

            Console.WriteLine(Toggle<AlwaysFalseWithAttribute>.IsEnabled()
                ? "Always false with attribute is true.  This should not be called."
                : "Always false toggle is false.  This should be called.");

            Console.WriteLine(Toggle<AppConfigWithAttribute>.IsEnabled()
                ? "Configuration toggle with attribute is true.  This should be called."
                : "Configuration toggle with attribute is false.  This should not be called.");

            Console.ReadKey();
        }
    }
}

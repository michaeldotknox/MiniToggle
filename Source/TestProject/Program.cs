using System;
using MiniToggle.Core;
using TestProject.Toggles;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize toggles
            Toggle<AlwaysTrue>.Is().AlwaysTrue();
            Toggle<AlwaysFalse>.Is().AlwaysFalse();
            Toggle<AppConfigToggle>.Is().Configured().WithSetting().Named("testToggle");

            if (Toggle<AlwaysTrue>.IsEnabled())
            {
                Console.WriteLine("Always true toggle is true.  This should be called.");
            }
            else
            {
                Console.WriteLine("Always true toggle is false.  This should not be called.");
            }

            if (Toggle<AlwaysFalse>.IsEnabled())
            {
                Console.WriteLine("Always false toggle is false.  This should not be called.");
            }
            else
            {
                Console.WriteLine("Always false toggle is false.  This should be called.");
            }
            if (Toggle<AppConfigToggle>.IsEnabled())
            {
                Console.WriteLine("Configuration toggle is true.  This should be called.");
            }
            else
            {
                Console.WriteLine("Configuration toggle is false.  This should not be called.");
            }

            Console.ReadKey();
        }
    }
}

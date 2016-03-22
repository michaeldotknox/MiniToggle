using System;

namespace MiniToggle.Core
{
    internal class ToggleDefinition
    {
        internal Type Type { get; set; }
        internal Func<bool> Evaluation { get; set; } 
    }
}

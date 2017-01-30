using System;

namespace MiniToggle.Core.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ToggleAttribute : Attribute
    {
        internal abstract ToggleDefinition GetDefinition(Type type);
    }
}

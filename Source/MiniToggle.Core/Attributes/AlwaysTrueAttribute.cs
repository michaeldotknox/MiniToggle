using System;

namespace MiniToggle.Core.Attributes
{
    /// <summary>
    /// Indicates that the toggle is always true
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AlwaysTrueAttribute : ToggleAttribute
    {
        internal override ToggleDefinition GetDefinition(Type type)
        {
            return new ToggleDefinition
            {
                Type = type,
                Evaluation = () => true
            };
        }
    }
}

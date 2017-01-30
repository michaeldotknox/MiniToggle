using System;

namespace MiniToggle.Core.Attributes
{
    /// <summary>
    /// Indicates that the toggle is always false
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AlwaysFalseAttribute : ToggleAttribute
    {
        internal override ToggleDefinition GetDefinition(Type type)
        {
            return new ToggleDefinition
            {
                Type = type,
                Evaluation = () => false
            };
        }
    }
}

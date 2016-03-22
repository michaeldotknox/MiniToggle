using System;

namespace MiniToggle.Core.Attributes
{
    /// <summary>
    /// Indicates that the toggle is always false
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class AlwaysFalseAttribute : Attribute
    {
    }
}

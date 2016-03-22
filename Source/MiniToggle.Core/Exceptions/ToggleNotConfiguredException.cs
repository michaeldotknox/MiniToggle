using System;

namespace MiniToggle.Core.Exceptions
{
    /// <summary>
    /// Exception that occurs when a toggle has not been configured either explicitly or with an attribute
    /// </summary>
    public class ToggleNotConfiguredException : Exception
    {
        internal ToggleNotConfiguredException(string toggleName)
            : base(
                $"The toggle named {toggleName} has not been configured.  Configure the toggle either with an attribute, or by explicitly configuring the toggle."
                )
        {
            
        }
    }
}

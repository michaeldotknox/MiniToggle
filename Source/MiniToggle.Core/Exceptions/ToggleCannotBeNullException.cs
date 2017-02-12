using System;

namespace MiniToggle.Core.Exceptions
{
    /// <summary>
    /// Indicates that the Toggle property of the object is null.
    /// </summary>
    public class ToggleCannotBeNullException : Exception
    {
        /// <summary>
        /// Creates a new instance of the exception
        /// </summary>
        public ToggleCannotBeNullException() : base("The toggle property cannot be null")
        {
            
        }
    }
}

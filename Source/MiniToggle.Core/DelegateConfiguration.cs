using System;

namespace MiniToggle.Core
{
    /// <summary>
    /// Holds information on a toggle that is configured to call a delegate
    /// </summary>
    public class DelegateConfiguration
    {
        internal Type Toggle { get; set; }
    }
}

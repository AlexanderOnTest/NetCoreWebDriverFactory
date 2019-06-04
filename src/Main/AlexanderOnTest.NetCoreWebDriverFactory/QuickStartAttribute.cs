using System;
using System.Collections.Generic;
using System.Text;

namespace AlexanderOnTest.NetCoreWebDriverFactory
{
    /// <summary>
    /// <para> Indicates that a this implementation should be used as a guide only and is not production ready.</para>
    /// <para> EXPLICITLY: This implementation may be changed without warning.</para>
    /// </summary>
    [AttributeUsage(
        AttributeTargets.All,
        AllowMultiple = false)]
    internal class QuickStartAttribute : Attribute
    {
    }
}

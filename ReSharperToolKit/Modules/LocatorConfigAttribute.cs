using System;

namespace ReSharperToolKit.Modules
{
    /// <summary>
    /// Place this attribute on any static method used to configure the Locator
    /// from inside your plugin.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class LocatorConfigAttribute : Attribute
    {
    }
}
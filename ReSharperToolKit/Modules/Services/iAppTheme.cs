﻿namespace ReSharperToolKit.Modules.Services
{
    /// <summary>
    /// Things related to the look and feel of the plugin.
    /// </summary>
    public interface iAppTheme
    {
        /// <summary>
        /// Adds the plugin name to end.
        /// </summary>
        string ActionText(string pText);
    }
}
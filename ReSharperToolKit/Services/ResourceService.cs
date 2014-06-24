using System;
using System.IO;

namespace ReSharperToolKit.Services
{
    /// <summary>
    /// Handles serializing resources embedded in the assembly of the plug-in.
    /// </summary>
    public static class ResourceService
    {
        /// <summary>
        /// Loads a resource as a string.
        /// </summary>
        public static string ReadAsString(Type pOwner, string pResourcePath)
        {
            using (Stream stream = pOwner.Assembly.GetManifestResourceStream(pResourcePath))
            {
                if (stream == null)
                {
                    throw new Exception(
                        string.Format(
                            "Resource not found {0} - Did you forget to embed the resource via it's properties?",
                            pResourcePath));
                }
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
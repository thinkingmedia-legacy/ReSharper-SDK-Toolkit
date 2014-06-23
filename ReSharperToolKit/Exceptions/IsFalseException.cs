using System;

namespace ReSharperToolKit.Exceptions
{
    /// <summary>
    /// A convenience exception to terminate a boolean evaluation.
    /// </summary>
    public class IsFalseException : Exception
    {
        public IsFalseException(string pMessage)
            : base(pMessage)
        {
        }
    }
}
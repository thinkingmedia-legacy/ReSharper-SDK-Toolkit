using System;

namespace ReSharperToolKit.Exceptions
{
    public class ProjectNotFoundException : Exception
    {
        public ProjectNotFoundException(string pMessage)
            : base(pMessage)
        {
        }
    }
}
using System;
using JetBrains.Annotations;

namespace ReSharperToolKit.Modules.Services.Impl
{
    public class StandardNamingPattern : iNamingService
    {
        /// <summary>
        /// Converts a namespace for a class into the namespace for the unit test.
        /// </summary>
        public string NameSpaceToTestNameSpace([NotNull] string pNameSpace)
        {
            if (pNameSpace == null)
            {
                throw new ArgumentNullException("pNameSpace");
            }

            // strip first namespace
            pNameSpace = pNameSpace.Substring(pNameSpace.IndexOf('.') + 1);
            // replace with Tests
            return string.Format("Tests.{0}", pNameSpace);
        }

        /// <summary>
        /// Converts a class name to unit test class.
        /// </summary>
        public string ClassNameToTest([NotNull] string pClassName)
        {
            if (pClassName == null)
            {
                throw new ArgumentNullException("pClassName");
            }

            return string.Format("{0}Test", pClassName);
        }

        /// <summary>
        /// Converts a project name into the test project.
        /// </summary>
        public string ProjectToTestProject([NotNull] string pProject)
        {
            if (pProject == null)
            {
                throw new ArgumentNullException("pProject");
            }

            return string.Format("{0}Tests", pProject);
        }
    }
}
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Psi.Tree;

namespace ReSharperToolKit.Modules.Services
{
    /// <summary>
    /// General services related to a project.
    /// </summary>
    public interface iProjectService
    {
        /// <summary>
        /// Attempts to convert a project file reference into a ReSharper compiled C sharp file.
        /// </summary>
        T getFileAs<T>(IProjectFile pFile) where T : class, IFile;

        /// <summary>
        /// Finds or creates a folder using a namespace.
        /// </summary>
        IProjectFolder getFolder(IProject pProject, string pNameSpace);
    }
}
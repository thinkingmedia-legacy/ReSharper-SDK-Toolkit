using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Util;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace ReSharperToolKit.Modules.Services.Impl
{
    /// <summary>
    /// General services related to a project.
    /// </summary>
    public class ProjectService : iProjectService
    {
        /// <summary>
        /// Finds or creates a folder using a namespace.
        /// </summary>
        public IProjectFolder getFolder(IProject pProject, string pNameSpace)
        {
            FileSystemPath path =
                FileSystemPath.Parse(pProject.ProjectFileLocation.Directory.FullPath + @"\" +
                                     pNameSpace.Replace(".", @"\"));
            return pProject.GetOrCreateProjectFolder(path);
        }

        /// <summary>
        /// Attempts to convert a project file reference into a derived IFile type. Such as ICSharpFile
        /// </summary>
        public T getFileAs<T>(IProjectFile pFile) where T : class, IFile
        {
            IPsiSourceFile sourceFile = pFile.ToSourceFile();
            if (sourceFile == null)
            {
                return null;
            }
            CachedPsiFile cachedPsiFile = sourceFile.GetPsiServices()
                .Files.GetCachedPsiFile(sourceFile, sourceFile.PrimaryPsiLanguage);
            if (cachedPsiFile == null)
            {
                return null;
            }
            return cachedPsiFile.PsiFile as T;
        }
    }
}
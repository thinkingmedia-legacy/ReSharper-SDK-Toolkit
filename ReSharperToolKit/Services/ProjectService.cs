using System;
using JetBrains.Annotations;
using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.Util;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.Files;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;

namespace ReSharperToolKit.Services
{
    /// <summary>
    /// General services related to a project.
    /// </summary>
    public static class ProjectService
    {
        /// <summary>
        /// Attempts to convert a project file reference into a derived IFile type. Such as ICSharpFile
        /// </summary>
        public static T getFileAs<T>([NotNull] IProjectFile pFile) where T : class, IFile
        {
            if (pFile == null)
            {
                throw new ArgumentNullException("pFile");
            }

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

        /// <summary>
        /// Provides access to the project file via a namespace pattern. If the file does not exist a new empty file is created.
        /// </summary>
        public static IProjectFile getFileOrCreate([NotNull] IProject pProject,
                                                   [NotNull] string pNameSpace,
                                                   [NotNull] string pFileNameWithExtension)
        {
            if (pProject == null)
            {
                throw new ArgumentNullException("pProject");
            }
            if (pNameSpace == null)
            {
                throw new ArgumentNullException("pNameSpace");
            }
            if (pFileNameWithExtension == null)
            {
                throw new ArgumentNullException("pFileNameWithExtension");
            }

            IProjectFolder folder = getFolder(pProject, pNameSpace);

            //TODO: Check if the file already exists.

            return AddNewItemUtil.AddFile(folder, pFileNameWithExtension);
        }

        /// <summary>
        /// Finds or creates a folder using a namespace.
        /// </summary>
        public static IProjectFolder getFolder([NotNull] IProject pProject,
                                               [NotNull] string pNameSpace)
        {
            if (pProject == null)
            {
                throw new ArgumentNullException("pProject");
            }
            if (pNameSpace == null)
            {
                throw new ArgumentNullException("pNameSpace");
            }

            FileSystemPath path =
                FileSystemPath.Parse(pProject.ProjectFileLocation.Directory.FullPath + @"\" +
                                     pNameSpace.Replace(".", @"\"));
            return pProject.GetOrCreateProjectFolder(path);
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        /// Provides access to the project file via a namespace pattern. If the file does not exist a new empty file is created.
        /// </summary>
        public static IProjectFile AddFile([NotNull] IProject pProject, [NotNull] string pNameSpace,
                                           [NotNull] string pFileNameWithExtension, [NotNull] string pFileContents)
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
            if (pFileContents == null)
            {
                throw new ArgumentNullException("pFileContents");
            }

            //TODO: Check if the file already exists.
            IProjectFolder folder = getFolder(pProject, pNameSpace);
            return AddNewItemUtil.AddFile(folder, pFileNameWithExtension, pFileContents);
        }

        /// <summary>
        /// Provides access to the project file via a namespace pattern. If the file does not exist a new empty file is created.
        /// </summary>
        public static TFileType AddFile<TFileType>([NotNull] IProject pProject, [NotNull] string pNameSpace,
                                                   [NotNull] string pFileNameWithExtension,
                                                   [NotNull] string pFileContents)
            where TFileType : class, IFile
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
            if (pFileContents == null)
            {
                throw new ArgumentNullException("pFileContents");
            }
            IProjectFile file = AddFile(pProject, pNameSpace, pFileNameWithExtension, pFileContents);
            IPsiSourceFile sourceFile = file.ToSourceFile();
            return sourceFile != null ? sourceFile.GetPrimaryPsiFile() as TFileType : null;
        }

        /// <summary>
        /// Checks if a file exists for a project using the namespace and filename.
        /// </summary>
        public static bool Exists([NotNull] IProject pProject, [NotNull] string pNameSpc, [NotNull] string pFilename)
        {
            if (pProject == null)
            {
                throw new ArgumentNullException("pProject");
            }
            if (pNameSpc == null)
            {
                throw new ArgumentNullException("pNameSpc");
            }
            if (pFilename == null)
            {
                throw new ArgumentNullException("pFilename");
            }
            string file = getFileName(pProject, pNameSpc, pFilename);
            return File.Exists(file);
        }

        /// <summary>
        /// Returns the disk location of a project file.
        /// </summary>
        public static string getFileName(IProject pProject, string pNameSpc, string pFilename)
        {
            FileSystemPath path = pProject.ProjectFileLocation;
            return path.Directory.FullPath + @"\" + pNameSpc.Replace(".", @"\") + @"\" + pFilename + ".cs";
        }

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
        /// Finds or creates a folder using a namespace.
        /// </summary>
        public static IProjectFolder getFolder([NotNull] IProject pProject, [NotNull] string pNameSpace)
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

        /// <summary>
        /// Collects all the source code files from all file references in a project. For example; all ICSharpFile references.
        /// </summary>
        public static List<TType> getSourceFiles<TType>([NotNull] IProject pProject)
            where TType : class, IFile
        {
            if (pProject == null)
            {
                throw new ArgumentNullException("pProject");
            }
            return (from file in pProject.GetAllProjectFiles()
                    let source = file.ToSourceFile()
                    where source != null
                    let code = source.GetPrimaryPsiFile() as TType
                    where code != null
                    select code).ToList();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;

namespace ReSharperToolKit.Services
{
    public static class SourceFileService
    {
        /// <summary>
        /// Adds a new class to the source file and provides an editor for it.
        /// </summary>
        public static void AddClass(CSharpElementFactory pFactory, ICSharpFile pFile, [NotNull] string pIdentifier,
                                    bool pPublic = true)
        {
            if (pIdentifier == null)
            {
                throw new ArgumentNullException("pIdentifier");
            }

            string @public = pPublic ? "public" : "internal";
            ICSharpFile tmpFile = pFactory.CreateFile(string.Format("{0} class {1} {{}}", @public, pIdentifier));
            pFile.AddTypeDeclarationAfter(tmpFile.TypeDeclarations[0], null);
        }

        /// <summary>
        /// Adds a using declaration to the using list.
        /// </summary>
        public static void AddUsing([NotNull] CSharpElementFactory pFactory, ICSharpFile pFile,
                                    [NotNull] string pNameSpace)
        {
            if (pFactory == null)
            {
                throw new ArgumentNullException("pFactory");
            }
            if (pNameSpace == null)
            {
                throw new ArgumentNullException("pNameSpace");
            }
            if (!HasUsing(pFile, pNameSpace))
            {
                IUsingDirective directive = pFactory.CreateUsingDirective(string.Format("using {0}", pNameSpace));
                pFile.AddImport(directive);
            }
        }

        /// <summary>
        /// Checks if a using declaration already exists in the file.
        /// </summary>
        public static bool HasUsing([NotNull] ICSharpFile pFile, [NotNull] string pNameSpace)
        {
            if (pFile == null)
            {
                throw new ArgumentNullException("pFile");
            }
            if (pNameSpace == null)
            {
                throw new ArgumentNullException("pNameSpace");
            }
            return Enumerable.Any(pFile.Imports, pUse=>pUse.ImportedSymbolName.QualifiedName == pNameSpace);
        }

        /// <summary>
        /// Finds all the tree nodes that implement a given interface in a file.
        /// </summary>
        public static List<TType> getAllNodesOf<TType>(ITreeNode pParent)
            where TType : class, ITreeNode
        {
            return (from node in pParent.EnumerateSubTree()
                    let decl = node as TType
                    where decl != null
                    select decl).ToList();
        }
    }
}
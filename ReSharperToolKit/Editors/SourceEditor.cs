using System;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace ReSharperToolKit.Editors
{
    public class SourceEditor
    {
        /// <summary>
        /// An element factory
        /// </summary>
        private readonly CSharpElementFactory _factory;

        /// <summary>
        /// The source code file.
        /// </summary>
        private readonly ICSharpFile _file;

        /// <summary>
        /// Constructor
        /// </summary>
        public SourceEditor([NotNull] CSharpElementFactory pFactory, [NotNull] ICSharpFile pFile)
        {
            if (pFactory == null)
            {
                throw new ArgumentNullException("pFactory");
            }
            if (pFile == null)
            {
                throw new ArgumentNullException("pFile");
            }

            _factory = pFactory;
            _file = pFile;
        }

        /// <summary>
        /// Adds a new class to the source file and provides an editor for it.
        /// </summary>
        public ClassEditor AddClass([NotNull] string pIdentifier, bool pPublic = true)
        {
            if (pIdentifier == null)
            {
                throw new ArgumentNullException("pIdentifier");
            }

            string @public = pPublic ? "public" : "internal";
            ICSharpFile tmpFile = _factory.CreateFile(string.Format("{0} class {1} {{}}", @public, pIdentifier));
            _file.AddTypeDeclarationAfter(tmpFile.TypeDeclarations[0], null);

            return new ClassEditor(_factory, (IClassDeclaration)_file.TypeDeclarations.Last());
        }

        /// <summary>
        /// Checks if a using declaration already exists in the file.
        /// </summary>
        public bool HasUsing([NotNull] string pNameSpace)
        {
            if (pNameSpace == null)
            {
                throw new ArgumentNullException("pNameSpace");
            }
            return Enumerable.Any(_file.Imports, pUse=>pUse.ImportedSymbolName.QualifiedName == pNameSpace);
        }

        /// <summary>
        /// Adds a using declaration to the using list.
        /// </summary>
        public void AddUsing([NotNull] string pNameSpace)
        {
            if (pNameSpace == null)
            {
                throw new ArgumentNullException("pNameSpace");
            }
            if (!HasUsing(pNameSpace))
            {
                IUsingDirective directive = _factory.CreateUsingDirective(string.Format("using {0}", pNameSpace));
                _file.AddImport(directive);
            }
        }
    }
}
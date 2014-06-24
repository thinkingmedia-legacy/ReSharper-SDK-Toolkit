using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using ReSharperToolKit.Modules;
using ReSharperToolKit.Modules.Factories;

namespace ReSharperToolKit.Editors.Impl
{
    public class SourceEditor : iSourceEditor
    {
        /// <summary>
        /// Used to provide editors as return values.
        /// </summary>
        private readonly iElementEditorFactory _editorFactory;

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

            _editorFactory = Locator.Get<iElementEditorFactory>();
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
            IUsingDirective directive = _factory.CreateUsingDirective(string.Format("using {0}", pNameSpace));
            _file.AddImport(directive);
        }

        /// <summary>
        /// Adds a new class to the source file and provides an editor for it.
        /// </summary>
        public iClassEditor AddClass([NotNull] string pIdentifier, bool pPublic = true)
        {
            if (pIdentifier == null)
            {
                throw new ArgumentNullException("pIdentifier");
            }

            string @public = pPublic ? "public" : "internal";
            ICSharpFile tmpFile = _factory.CreateFile(string.Format("{0} class {1} {{}}", @public, pIdentifier));
            _file.AddTypeDeclarationAfter(tmpFile.TypeDeclarations[0], null);
            return _editorFactory.CreateClassEditor(_factory, _file.TypeDeclarations.Last() as IClassDeclaration);
        }
    }
}
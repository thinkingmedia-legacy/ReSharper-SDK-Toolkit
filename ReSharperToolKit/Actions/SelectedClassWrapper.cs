using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using ReSharperToolKit.Editors;

namespace ReSharperToolKit.Actions
{
    /// <summary>
    /// Holds data associated with the currently selected class.
    /// </summary>
    public class SelectedClassWrapper
    {
        /// <summary>
        /// Used to edit the current class.
        /// </summary>
        public readonly ClassEditor ClassEditor;

        /// <summary>
        /// The identifier for the current class.
        /// </summary>
        public readonly string ClassName;

        /// <summary>
        /// This is the current class declaration for the action.
        /// </summary>
        public readonly IClassDeclaration Decl;

        /// <summary>
        /// Used to edit the C source file.
        /// </summary>
        public readonly SourceEditor SourceEditor;

        /// <summary>
        /// Constructor
        /// </summary>
        public SelectedClassWrapper([NotNull] CSharpElementFactory pFactory, 
                                    [NotNull] ICSharpFile pFile,
                                    [NotNull] IClassDeclaration pDecl)
        {
            if (pFactory == null)
            {
                throw new ArgumentNullException("pFactory");
            }
            if (pFile == null)
            {
                throw new ArgumentNullException("pFile");
            }
            if (pDecl == null)
            {
                throw new ArgumentNullException("pDecl");
            }

            Decl = pDecl;

            ClassName = Decl.NameIdentifier.Name;

            ClassEditor = new ClassEditor(pFactory, Decl);
            SourceEditor = new SourceEditor(pFactory, pFile);
        }
    }
}
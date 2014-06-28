using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace ReSharperToolKit.Actions
{
    /// <summary>
    /// Holds data associated with the currently selected class.
    /// </summary>
    public class SelectedClassWrapper
    {
        /// <summary>
        /// The identifier for the current class.
        /// </summary>
        public readonly string ClassName;

        /// <summary>
        /// This is the current class declaration for the action.
        /// </summary>
        public readonly IClassDeclaration Decl;

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
        }
    }
}
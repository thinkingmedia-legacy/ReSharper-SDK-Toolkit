using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using ReSharperToolKit.Editors;
using ReSharperToolKit.Editors.Impl;

namespace ReSharperToolKit.Modules.Factories.Impl
{
    public class ElementEditorFactory : iElementEditorFactory
    {
        /// <summary>
        /// Create an editor for class declarations.
        /// </summary>
        /// <param name="pFactory"></param>
        /// <param name="pClass"></param>
        public iClassEditor CreateClassEditor([NotNull] CSharpElementFactory pFactory,
                                              [NotNull] IClassDeclaration pClass)
        {
            if (pFactory == null)
            {
                throw new ArgumentNullException("pFactory");
            }
            if (pClass == null)
            {
                throw new ArgumentNullException("pClass");
            }
            return new ClassEditor(pFactory, pClass);
        }

        /// <summary>
        /// Create an editor for source files.
        /// </summary>
        /// <param name="pFactory"></param>
        /// <param name="pFile"></param>
        public iSourceEditor CreateSourceEditor([NotNull] CSharpElementFactory pFactory, [NotNull] ICSharpFile pFile)
        {
            if (pFactory == null)
            {
                throw new ArgumentNullException("pFactory");
            }
            if (pFile == null)
            {
                throw new ArgumentNullException("pFile");
            }
            return new SourceEditor(pFactory, pFile);
        }
    }
}
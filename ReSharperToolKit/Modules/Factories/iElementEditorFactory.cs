using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using ReSharperToolKit.Editors;

namespace ReSharperToolKit.Modules.Factories
{
    public interface iElementEditorFactory
    {
        /// <summary>
        /// Create an editor for class declarations.
        /// </summary>
        iClassEditor CreateClassEditor(CSharpElementFactory pFactory, IClassDeclaration pClass);

        /// <summary>
        /// Create an editor for source files.
        /// </summary>
        iSourceEditor CreateSourceEditor(CSharpElementFactory pFactory, ICSharpFile pFile);
    }
}
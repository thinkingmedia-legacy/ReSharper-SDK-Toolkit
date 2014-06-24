using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using ReSharperToolKit.Editors;
using ReSharperToolKit.Modules;
using ReSharperToolKit.Modules.Factories;
using ReSharperToolKit.Modules.Services;

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
        public readonly iClassEditor ClassEditor;

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
        public readonly iSourceEditor SourceEditor;

        /// <summary>
        /// The identifier for the unit test of the class.
        /// </summary>
        public readonly string UnitTestName;

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

            iNamingService namingService = Locator.Get<iNamingService>();
            iElementEditorFactory factory = Locator.Get<iElementEditorFactory>();

            ClassName = Decl.NameIdentifier.Name;
            UnitTestName = namingService.ClassNameToTest(ClassName);

            ClassEditor = factory.CreateClassEditor(pFactory, Decl);
            SourceEditor = factory.CreateSourceEditor(pFactory, pFile);
        }
    }
}
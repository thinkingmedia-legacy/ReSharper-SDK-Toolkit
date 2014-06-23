using JetBrains.ProjectModel;
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using ReSharperToolKit.Exceptions;
using ReSharperToolKit.Modules;
using ReSharperToolKit.Modules.Services;

namespace ReSharperToolKit.Actions
{
    public abstract class ClassActionBase : ContextActionBase
    {
        /// <summary>
        /// Not null if editing a C# file.
        /// </summary>
        protected readonly ICSharpContextActionDataProvider Provider;

        /// <summary>
        /// The current theme
        /// </summary>
        protected readonly iAppTheme Theme;

        /// <summary>
        /// Services related to the unit test.
        /// </summary>
        protected readonly iUnitTestService UnitTestService;

        /// <summary>
        /// Services related to the target test project.
        /// </summary>
        private readonly iTestProjectService _testProjectService;

        /// <summary>
        /// Services related to the node tree.
        /// </summary>
        private readonly iTreeNodeService _treeNodeService;

        /// <summary>
        /// The currently selected class.
        /// </summary>
        protected SelectedClassWrapper SelectedClass;

        /// <summary>
        /// Constructor
        /// </summary>
        protected ClassActionBase(ICSharpContextActionDataProvider pProvider)
        {
            Provider = pProvider;
            SelectedClass = null;

            Theme = Locator.Get<iAppTheme>();
            UnitTestService = Locator.Get<iUnitTestService>();

            _testProjectService = Locator.Get<iTestProjectService>();
            _treeNodeService = Locator.Get<iTreeNodeService>();
        }

        /// <summary>
        /// Ask the derived type if this action can be performed on this class.
        /// </summary>
        protected abstract bool isAvailableForClass(IUserDataHolder pCache, IProject pTestProject,
                                                    IClassDeclaration pClass);

        /// <summary>
        /// Checks if a unit test exists for the current class declaration.
        /// </summary>
        public override bool IsAvailable(IUserDataHolder pCache)
        {
            try
            {
                ThrowIf.Null(Provider);
                ITreeNode node = ThrowIf.Null(Provider.SelectedElement);
                IProject testProejct = _testProjectService.getProject(Provider.Project);
                CSharpElementFactory factory = CSharpElementFactory.GetInstance(Provider.PsiModule);

                IClassDeclaration decl = ThrowIf.Null(_treeNodeService.isClassIdentifier(node));
                SelectedClass = new SelectedClassWrapper(factory, Provider.PsiFile, decl);

                ThrowIf.False(isAvailableForClass(pCache, testProejct, decl));
                return true;
            }
            catch (IsFalseException)
            {
                SelectedClass = null;
                return false;
            }
        }
    }
}
using JetBrains.ReSharper.Feature.Services.CSharp.Bulbs;
using JetBrains.ReSharper.Intentions.Extensibility;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Tree;
using JetBrains.Util;
using ReSharperToolKit.Exceptions;
using ReSharperToolKit.Services;

namespace ReSharperToolKit.Actions
{
    public abstract class ClassActionBase : ContextActionBase
    {
        /// <summary>
        /// Not null if editing a C# file.
        /// </summary>
        protected readonly ICSharpContextActionDataProvider Provider;

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
        }

        /// <summary>
        /// Ask the derived type if this action can be performed on this class.
        /// </summary>
        protected abstract bool isAvailableForClass(IUserDataHolder pCache);

        /// <summary>
        /// Checks if a unit test exists for the current class declaration.
        /// </summary>
        public override bool IsAvailable(IUserDataHolder pCache)
        {
            try
            {
                ThrowIf.Null(Provider);
                ITreeNode node = ThrowIf.Null(Provider.SelectedElement);
                CSharpElementFactory factory = CSharpElementFactory.GetInstance(Provider.PsiModule);

                IClassDeclaration decl = ThrowIf.Null(TreeNodeService.isClassIdentifier(node));
                SelectedClass = new SelectedClassWrapper(factory, Provider.PsiFile, decl);

                ThrowIf.False(isAvailableForClass(pCache));
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
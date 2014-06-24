using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace ReSharperToolKit.Editors.Impl
{
    public class ClassEditor : iClassEditor
    {
        /// <summary>
        /// The class being edited.
        /// </summary>
        private readonly IClassDeclaration _class;

        /// <summary>
        /// C sharp element factory
        /// </summary>
        private readonly CSharpElementFactory _factory;

        /// <summary>
        /// Constructor
        /// </summary>
        public ClassEditor([NotNull] CSharpElementFactory pFactory, [NotNull] IClassDeclaration pClass)
        {
            if (pFactory == null)
            {
                throw new ArgumentNullException("pFactory");
            }
            if (pClass == null)
            {
                throw new ArgumentNullException("pClass");
            }
            _class = pClass;
            _factory = pFactory;
        }

        /// <summary>
        /// Adds a new attribute to the class declaration.
        /// </summary>
        /// <param name="pAttribute">The attribute as source code excluding the []</param>
        public void AddAttribute([NotNull] string pAttribute)
        {
            if (pAttribute == null)
            {
                throw new ArgumentNullException("pAttribute");
            }
            ICSharpTypeMemberDeclaration tmpClass =
                _factory.CreateTypeMemberDeclaration(string.Format("[{0}] class C{{}}", pAttribute));
            _class.AddAttributeAfter(tmpClass.Attributes[0], null);
        }
    }
}
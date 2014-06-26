using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Tree;

namespace ReSharperToolKit.Editors
{
    public class ClassEditor
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
        /// Finds all the attributes assigned to a class using the attributes type.
        /// </summary>
        public static IEnumerable<IAttribute> getAttributes<TAttributeType>(IClassDeclaration pClass)
            where TAttributeType : Attribute
        {
            return
                pClass.Attributes.Where(
                    pAttr => pAttr.Name.QualifiedName + "Attribute" == typeof(TAttributeType).Name);
        }

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
        /// Checks if a class has a custom attribute assigned to it.
        /// </summary>
        public bool HasAttribute([NotNull] Type pAttributeType)
        {
            if (pAttributeType == null)
            {
                throw new ArgumentNullException("pAttributeType");
            }

            return Enumerable.Any(_class.Attributes, pAttr=>pAttr.Name.QualifiedName + "Attribute" == pAttributeType.Name);
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
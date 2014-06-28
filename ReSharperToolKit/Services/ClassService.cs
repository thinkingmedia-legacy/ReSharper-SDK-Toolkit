using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.ReSharper.Psi.CSharp.Parsing;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Parsing;

namespace ReSharperToolKit.Services
{
    public static class ClassService
    {
        /// <summary>
        /// Adds a new attribute to the class declaration.
        /// </summary>
        public static void AddAttribute([NotNull] CSharpElementFactory pFactory, [NotNull] IClassDeclaration pClass,
                                        [NotNull] string pAttribute)
        {
            if (pFactory == null)
            {
                throw new ArgumentNullException("pFactory");
            }
            if (pClass == null)
            {
                throw new ArgumentNullException("pClass");
            }
            if (pAttribute == null)
            {
                throw new ArgumentNullException("pAttribute");
            }
            ICSharpTypeMemberDeclaration tmpClass =
                pFactory.CreateTypeMemberDeclaration(String.Format("[{0}] class C{{}}", pAttribute));
            pClass.AddAttributeAfter(tmpClass.Attributes[0], null);
        }

        /// <summary>
        /// Checks if a class has a custom attribute assigned to it.
        /// </summary>
        public static bool HasAttribute([NotNull] IClassDeclaration pClass, [NotNull] Type pAttributeType)
        {
            if (pClass == null)
            {
                throw new ArgumentNullException("pClass");
            }
            if (pAttributeType == null)
            {
                throw new ArgumentNullException("pAttributeType");
            }

            return Enumerable.Any(pClass.Attributes,
                pAttr=>pAttr.Name.QualifiedName + "Attribute" == pAttributeType.Name);
        }

        /// <summary>
        /// Finds all the attributes assigned to a class using the attributes type.
        /// </summary>
        public static IEnumerable<IAttribute> getAttributes<TAttributeType>(IClassDeclaration pClass)
            where TAttributeType : Attribute
        {
            return
                pClass.Attributes.Where(
                    pAttr=>pAttr.Name.QualifiedName + "Attribute" == typeof (TAttributeType).Name);
        }

        public static string getFullNameSpace([NotNull] ICSharpTypeDeclaration pType)
        {
            if (pType == null)
            {
                throw new ArgumentNullException("pType");
            }
            return pType.OwnerNamespaceDeclaration.QualifiedName;
        }

        /// <summary>
        /// Checks that a class can be tested.
        /// </summary>
        public static bool isSafelyPublic([CanBeNull] IModifiersList pModifiers)
        {
            if (pModifiers == null)
            {
                return false;
            }

            TokenNodeType[] mustHave = {CSharpTokenType.PUBLIC_KEYWORD};
            TokenNodeType[] mustNotHave =
            {
                CSharpTokenType.ABSTRACT_KEYWORD,
                CSharpTokenType.INTERNAL_KEYWORD,
                CSharpTokenType.PARTIAL_KEYWORD
            };

            List<TokenNodeType> types = pModifiers.Modifiers.Select(pMod=>pMod.GetTokenType()).ToList();

            return !types.Any(pTypeB=>mustNotHave.Any(pTypeA=>pTypeA == pTypeB))
                   && types.Any(pTypeB=>mustHave.Any(pTypeA=>pTypeA == pTypeB));
        }
    }
}
using System;
using JetBrains.Annotations;
using JetBrains.ReSharper.Psi.CSharp.Parsing;
using JetBrains.ReSharper.Psi.CSharp.Tree;
using JetBrains.ReSharper.Psi.Parsing;
using JetBrains.ReSharper.Psi.Tree;

namespace ReSharperToolKit.Services
{
    public static class TreeNodeService
    {
        /// <summary>
        /// Checks if the node is an identifier for a class, and returns the declaration if it is.
        /// </summary>
        public static IClassDeclaration isClassIdentifier([NotNull] ITreeNode pNode)
        {
            if (pNode == null)
            {
                throw new ArgumentNullException("pNode");
            }

            return isType(pNode, CSharpTokenType.IDENTIFIER)
                ? pNode.Parent as IClassDeclaration
                : null;
        }

        /// <summary>
        /// Checks if a node is a type of C# token.
        /// </summary>
        public static bool isType([CanBeNull] ITreeNode pNode, [NotNull] TokenNodeType pToken)
        {
            if (pToken == null)
            {
                throw new ArgumentNullException("pToken");
            }
            return pNode != null && pNode.NodeType == pToken;
        }
    }
}
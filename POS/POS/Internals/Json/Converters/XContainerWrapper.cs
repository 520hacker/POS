using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Lib.JSON.Converters
{
    internal class XContainerWrapper : XObjectWrapper
    {
        private XContainer Container
        {
            get
            {
                return (XContainer)this.WrappedNode;
            }
        }

        public XContainerWrapper(XContainer container) : base(container)
        {
        }

        public override IList<IXmlNode> ChildNodes
        {
            get
            {
                return this.Container.Nodes().Select(n => WrapNode(n)).ToList();
            }
        }

        public override IXmlNode ParentNode
        {
            get
            {
                if (this.Container.Parent == null)
                {
                    return null;
                }
          
                return WrapNode(this.Container.Parent);
            }
        }

        internal static IXmlNode WrapNode(XObject node)
        {
            if (node is XDocument)
            {
                return new XDocumentWrapper((XDocument)node);
            }
            else if (node is XElement)
            {
                return new XElementWrapper((XElement)node);
            }
            else if (node is XContainer)
            {
                return new XContainerWrapper((XContainer)node);
            }
            else if (node is XProcessingInstruction)
            {
                return new XProcessingInstructionWrapper((XProcessingInstruction)node);
            }
            else if (node is XText)
            {
                return new XTextWrapper((XText)node);
            }
            else if (node is XComment)
            {
                return new XCommentWrapper((XComment)node);
            }
            else if (node is XAttribute)
            {
                return new XAttributeWrapper((XAttribute)node);
            }
            else
            {
                return new XObjectWrapper(node);
            }
        }

        public override IXmlNode AppendChild(IXmlNode newChild)
        {
            this.Container.Add(newChild.WrappedNode);
            return newChild;
        }
    }
}
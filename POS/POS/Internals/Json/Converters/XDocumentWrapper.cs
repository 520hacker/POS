using System.Collections.Generic;
using System.Xml.Linq;
using Lib.JSON.Utilities;

namespace Lib.JSON.Converters
{
    internal class XDocumentWrapper : XContainerWrapper, IXmlDocument
    {
        private XDocument Document
        {
            get
            {
                return (XDocument)this.WrappedNode;
            }
        }

        public XDocumentWrapper(XDocument document) : base(document)
        {
        }

        public override IList<IXmlNode> ChildNodes
        {
            get
            {
                IList<IXmlNode> childNodes = base.ChildNodes;

                if (this.Document.Declaration != null)
                {
                    childNodes.Insert(0, new XDeclarationWrapper(this.Document.Declaration));
                }

                return childNodes;
            }
        }

        public IXmlNode CreateComment(string text)
        {
            return new XObjectWrapper(new XComment(text));
        }

        public IXmlNode CreateTextNode(string text)
        {
            return new XObjectWrapper(new XText(text));
        }

        public IXmlNode CreateCDataSection(string data)
        {
            return new XObjectWrapper(new XCData(data));
        }

        public IXmlNode CreateWhitespace(string text)
        {
            return new XObjectWrapper(new XText(text));
        }

        public IXmlNode CreateSignificantWhitespace(string text)
        {
            return new XObjectWrapper(new XText(text));
        }

        public IXmlNode CreateXmlDeclaration(string version, string encoding, string standalone)
        {
            return new XDeclarationWrapper(new XDeclaration(version, encoding, standalone));
        }

        public IXmlNode CreateProcessingInstruction(string target, string data)
        {
            return new XProcessingInstructionWrapper(new XProcessingInstruction(target, data));
        }

        public IXmlElement CreateElement(string elementName)
        {
            return new XElementWrapper(new XElement(elementName));
        }

        public IXmlElement CreateElement(string qualifiedName, string namespaceUri)
        {
            string localName = MiscellaneousUtils.GetLocalName(qualifiedName);
            return new XElementWrapper(new XElement(XName.Get(localName, namespaceUri)));
        }

        public IXmlNode CreateAttribute(string name, string value)
        {
            return new XAttributeWrapper(new XAttribute(name, value));
        }

        public IXmlNode CreateAttribute(string qualifiedName, string namespaceUri, string value)
        {
            string localName = MiscellaneousUtils.GetLocalName(qualifiedName);
            return new XAttributeWrapper(new XAttribute(XName.Get(localName, namespaceUri), value));
        }

        public IXmlElement DocumentElement
        {
            get
            {
                if (this.Document.Root == null)
                {
                    return null;
                }

                return new XElementWrapper(this.Document.Root);
            }
        }

        public override IXmlNode AppendChild(IXmlNode newChild)
        {
            XDeclarationWrapper declarationWrapper = newChild as XDeclarationWrapper;
            if (declarationWrapper != null)
            {
                this.Document.Declaration = declarationWrapper.Declaration;
                return declarationWrapper;
            }
            else
            {
                return base.AppendChild(newChild);
            }
        }
    }
}
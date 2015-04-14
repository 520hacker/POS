using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Lib.JSON.Converters
{
    internal class XmlNodeWrapper : IXmlNode
    {
        private readonly XmlNode _node;

        public XmlNodeWrapper(XmlNode node)
        {
            this._node = node;
        }

        public object WrappedNode
        {
            get
            {
                return this._node;
            }
        }

        public XmlNodeType NodeType
        {
            get
            {
                return this._node.NodeType;
            }
        }

        public string Name
        {
            get
            {
                return this._node.Name;
            }
        }

        public string LocalName
        {
            get
            {
                return this._node.LocalName;
            }
        }

        public IList<IXmlNode> ChildNodes
        {
            get
            {
                return this._node.ChildNodes.Cast<XmlNode>().Select(n => this.WrapNode(n)).ToList();
            }
        }

        private IXmlNode WrapNode(XmlNode node)
        {
            switch (node.NodeType)
            {
                case XmlNodeType.Element:
                    return new XmlElementWrapper((XmlElement)node);
                case XmlNodeType.XmlDeclaration:
                    return new XmlDeclarationWrapper((XmlDeclaration)node);
                default:
                    return new XmlNodeWrapper(node);
            }
        }

        public IList<IXmlNode> Attributes
        {
            get
            {
                if (this._node.Attributes == null)
                {
                    return null;
                }

                return this._node.Attributes.Cast<XmlAttribute>().Select(a => this.WrapNode(a)).ToList();
            }
        }

        public IXmlNode ParentNode
        {
            get
            {
                XmlNode node = (this._node is XmlAttribute)
                               ? ((XmlAttribute)this._node).OwnerElement
                               : this._node.ParentNode;
          
                if (node == null)
                {
                    return null;
                }

                return this.WrapNode(node);
            }
        }

        public string Value
        {
            get
            {
                return this._node.Value;
            }
            set
            {
                this._node.Value = value;
            }
        }

        public IXmlNode AppendChild(IXmlNode newChild)
        {
            XmlNodeWrapper xmlNodeWrapper = (XmlNodeWrapper)newChild;
            this._node.AppendChild(xmlNodeWrapper._node);

            return newChild;
        }

        public string Prefix
        {
            get
            {
                return this._node.Prefix;
            }
        }

        public string NamespaceUri
        {
            get
            {
                return this._node.NamespaceURI;
            }
        }
    }
}
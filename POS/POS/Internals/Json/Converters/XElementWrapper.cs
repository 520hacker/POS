using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Lib.JSON.Converters
{
    internal class XElementWrapper : XContainerWrapper, IXmlElement
    {
        private XElement Element
        {
            get
            {
                return (XElement)this.WrappedNode;
            }
        }

        public XElementWrapper(XElement element) : base(element)
        {
        }

        public void SetAttributeNode(IXmlNode attribute)
        {
            XObjectWrapper wrapper = (XObjectWrapper)attribute;
            this.Element.Add(wrapper.WrappedNode);
        }

        public override IList<IXmlNode> Attributes
        {
            get
            {
                return this.Element.Attributes().Select(a => new XAttributeWrapper(a)).Cast<IXmlNode>().ToList();
            }
        }

        public override string Value
        {
            get
            {
                return this.Element.Value;
            }
            set
            {
                this.Element.Value = value;
            }
        }

        public override string LocalName
        {
            get
            {
                return this.Element.Name.LocalName;
            }
        }

        public override string NamespaceUri
        {
            get
            {
                return this.Element.Name.NamespaceName;
            }
        }

        public string GetPrefixOfNamespace(string namespaceUri)
        {
            return this.Element.GetPrefixOfNamespace(namespaceUri);
        }
    }
}